// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using PostSharp.Engineering.BuildTools.Build;
using PostSharp.Engineering.BuildTools.Build.Model;
using PostSharp.Engineering.BuildTools.Utilities;
using System.IO;
using System.IO.Compression;

namespace BuildPostSharpDocumentation
{
    public class DocFxSolution : Solution
    {
        public string DocFxSiteDirectory { get; }

        public string PackageFileName { get; }

        public DocFxSolution( string solutionPath, string docFxSiteDirectory, string packageFileName ) : base( solutionPath )
        {
            this.DocFxSiteDirectory = docFxSiteDirectory;
            this.PackageFileName = packageFileName;

            // Packing is done by the publish command.
            this.BuildMethod = PostSharp.Engineering.BuildTools.Build.Model.BuildMethod.Pack;            
        }

        public override bool Build( BuildContext context, BuildSettings settings )
        {
            if ( !RunDocFx( context, this.SolutionPath, "metadata" ) )
            {
                return false;
            }

            if ( !RunDocFx( context, this.SolutionPath, "build" ) )
            {
                return false;
            }

            return true;
        }

        private static bool RunDocFx( BuildContext context, string solutionPath, string command )
        {
            return ToolInvocationHelper.InvokeTool(
                context.Console,
                Path.Combine( context.RepoDirectory, "docfx\\packages\\docfx.console.2.59.0\\tools\\docfx.exe" ),
                command + " " + Path.Combine( context.RepoDirectory, solutionPath ),
                context.RepoDirectory );
        }

        public override bool Pack( BuildContext context, BuildSettings settings )
        {
            if ( !this.Build( context, settings ))
            {
                return false;
            }

            ZipFile.CreateFromDirectory(
                Path.Combine( context.RepoDirectory, "docfx", this.DocFxSiteDirectory ),
                Path.Combine( context.RepoDirectory, "artifacts", "publish", "private", this.PackageFileName ) );

            return true;
        }

        public override bool Test( BuildContext context, BuildSettings settings )
        {
            return true;
        }

        public override bool Restore( BuildContext context, BuildSettings settings )
        {
            return DotNetHelper.Run(context, settings, Path.Combine( context.RepoDirectory, "docfx", "DocFx.csproj" ), "restore");
        }
    }
}