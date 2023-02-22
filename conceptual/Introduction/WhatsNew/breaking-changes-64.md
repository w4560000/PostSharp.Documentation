---
uid: breaking-changes-64
title: "Breaking Changes in PostSharp 6.4"
product: "postsharp"
categories: "PostSharp;AOP;Metaprogramming"
---
# Breaking Changes in PostSharp 6.4


## LocationInterceptionAspect and initializers of static fields and properties

The <xref:PostSharp.Aspects.LocationInterceptionAspect.OnSetValue(PostSharp.Aspects.LocationInterceptionArgs)> advice will now be invoked for initializers of static fields and properties. These assignments were previously ignored. The breaking change does not affect binaries that have already been compiled. It only affects new compilations. 

