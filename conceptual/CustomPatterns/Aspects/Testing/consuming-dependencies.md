---
uid: consuming-dependencies
title: "Consuming Dependencies from an Aspect"
product: "postsharp"
categories: "PostSharp;AOP;Metaprogramming"
---
# Consuming Dependencies from an Aspect

Aspects, as other components, may have dependencies to other application services. Aspects may be bound to the abstract interface to this service, and may need to resolve the dependency at run time.

However, two reasons prevent us from the following approaches that are usual with dependency injection containers:

* Aspects are instantiated at build time, and dependency-injection containers only exist at run-time.

* Aspects typically have a static scope. Unless they implement the <xref:PostSharp.Aspects.IInstanceScopedAspect>, aspect instances are stored in static fields, even when applied to instance members. 

These characteristics are not an obstacle to using service containers, but different patterns must be followed.

This section presents several ways to consume dependencies from an aspect:

* <xref:global-service-container>
* <xref:global-service-locator>
* <xref:dynamic-dependency-resolution>
* <xref:contextual-dependency-resolution>
* <xref:importing-dependencies>
