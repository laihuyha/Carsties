# Services
- Every Service ***must be add before*** `var app = builder.Build();` run otherwise will not work cause after that code DI container is considered frozen.
- After `var app = builder.Build();` mean ***fully builded***. The `IServiceCollection` interface is used for building a dependency injection container gets composed to an `IServiceProvider` instance which you can use to resolve services via `ApplicationServices` within `IApplicationBuilder ` or `RequestServices` within `HttpContext`. <br><br> > **Ref :** [Resolving instances with ASP.NET Core DI from within ConfigureServices](https://stackoverflow.com/questions/32459670/resolving-instances-with-asp-net-core-di-from-within-configureservices)
# App Host LifeTime
```
app.Lifetime.ApplicationStarted.Register(async () =>
{
    await app.UseAppBuilderExtension(builder.Configuration);
});
```
<br> Using app lifetime for delay start synchronous data from Auction Service. `LifeTime.ApplicationStarted` triggered when the application host has fully started then we *register* use `Register` method for creating a lambda function inside callback extension method.