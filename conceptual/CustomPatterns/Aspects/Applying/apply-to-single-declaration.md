---
uid: apply-to-single-declaration
title: "Adding Aspects to a Single Declaration Using Attributes"
product: "postsharp"
categories: "PostSharp;AOP;Metaprogramming"
---
# Adding Aspects to a Single Declaration Using Attributes

Aspects in PostSharp are plain custom attributes. You can apply them to any element of code as usual.

In the following example, the `Trace` aspect is applied to two methods. 

```csharp
public class CustomerService
          {
              [Trace]
              public Custom GetCustomer( int customerId )
              {
                  // Details skipped.
              }
              
              [Trace]
              public void MergeCustomers( Customer customer1, Customer customer2 );
              {
                  // Details skipped.
              }
          }
```

