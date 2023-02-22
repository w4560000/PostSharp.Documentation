// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Amazon;
using BuildPostSharpDocumentation;
using PostSharp.Engineering.BuildTools;
using PostSharp.Engineering.BuildTools.Build.Solutions;
using PostSharp.Engineering.BuildTools.AWS.S3.Publishers;
using PostSharp.Engineering.BuildTools.Build;
using PostSharp.Engineering.BuildTools.Build.Model;
using PostSharp.Engineering.BuildTools.Dependencies.Model;
using PostSharp.Engineering.BuildTools.Utilities;
using Spectre.Console.Cli;
using System.IO;
using System.Diagnostics;
using PostSharp.Engineering.BuildTools.Build.Publishers;

const string docPackageFileName = "PostSharp.Doc.zip";

// PostSharp SDK Documentiation is not published at the moment.
// const string docSdkPackageFileName = "PostSharp.Sdk.Doc.zip";

// TODO: Replace with PostSharpDocumentation dependency.
var product = new Product( new( "PostSharp.Documentation", VcsProvider.GitHub, "PostSharp", false ) )
{
    Solutions = new Solution[]
    {
        new DotNetSolution( Path.Combine( "code", "PostSharp.Documentation.Prerequisites.sln" ) ) { CanFormatCode = true },
        new DocFxSolution( Path.Combine( "docfx", "docfx.json" ), "_site", docPackageFileName ),

        // PostSharp SDK Documentiation is not published at the moment.
        // new DocFxSolution( Path.Combine( "docfx", "docfx_sdk.json"), "_site_sdk", docSdkPackageFileName )
    },
    PublicArtifacts = Pattern.Create(
        docPackageFileName

        // PostSharp SDK Documentiation is not published at the moment.
        // docSdkPackageFileName
    ),
    Dependencies = new[] { Dependencies.PostSharpEngineering },
    AdditionalDirectoriesToClean = new[] { "docfx\\obj", "docfx\\_site", "docfx\\_site_sdk" },

    // Disable automatic build triggers.
    Configurations = Product.DefaultConfigurations
        .WithValue( BuildConfiguration.Debug, c => c with { BuildTriggers = default } )
        .WithValue( BuildConfiguration.Public, new BuildConfigurationInfo(
            MSBuildName: "Release",
            PublicPublishers: new Publisher[]
            {
                new MergePublisher(),
                new DocumentationPublisher( new S3PublisherConfiguration[]
                {
                    new(docPackageFileName, RegionEndpoint.EUWest1, "doc.postsharp.net", docPackageFileName),

                    // PostSharp SDK Documentiation is not published at the moment.
                    //new(docSdkPackageFileName, RegionEndpoint.EUWest1, "doc.postsharp.net", docSdkPackageFileName),
                } )
            } ) )
};

product.PrepareCompleted += OnPrepareCompleted;


var commandApp = new CommandApp();

commandApp.AddProductCommands( product );

return commandApp.Run( args );


static void OnPrepareCompleted( PrepareCompletedEventArgs args )
{
    var nuget = Path.Combine( Path.GetDirectoryName( Process.GetCurrentProcess().MainModule!.FileName )!, "nuget.exe " );

    if ( !ToolInvocationHelper.InvokeTool( args.Context.Console, nuget,
        "restore \"docfx\\packages.config\" -OutputDirectory \"docfx\\packages\"", args.Context.RepoDirectory ) )
    {
        args.IsFailed = true;
    }
}