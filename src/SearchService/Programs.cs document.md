# Services
- Every Service ***must be add before*** `var app = builder.Build();` run otherwise will not work cause after that code DI container is considered frozen.
- After `var app = builder.Build();` mean ***fully builded***. The `IServiceCollection` interface is used for building a dependency injection container gets composed to an `IServiceProvider` instance which you can use to resolve services via `ApplicationServices` within `IApplicationBuilder ` or `RequestServices` within `HttpContext`. <br><br> > **Ref :** [Resolving instances with ASP.NET Core DI from within ConfigureServices](https://stackoverflow.com/questions/32459670/resolving-instances-with-asp-net-core-di-from-within-configureservices)<br>

# App Host LifeTime
```
app.Lifetime.ApplicationStarted.Register(async () =>
{
    await app.UseAppBuilderExtension(builder.Configuration);
});
```
<br> Using app lifetime for delay start synchronous data from Auction Service. `LifeTime.ApplicationStarted` triggered when the application host has fully started then we ***register*** a lambda function that use `Register` method for creating a lambda function inside callback extension method.<br>

# MassTransit
- First of all, we need to talk about the way that the microservice communicates with each other. There some ways like HTTP Communication,
Event-driven, Message Communication and in this project we use Message Communication to do this.<br>
- **Message Queue** : A message queue is a type of architecture that facilitates asynchronous communication. In this context, the term "queue" refers to a sequence of messages that are waiting to be processed in the order they were received, based on the first in, first out (FIFO) principle. A message is essentially data that needs to be transmitted from a sender to a recipient.<br>
  > **Ref :** [Message Queue](https://en.wikipedia.org/wiki/Message_queue)<br>
  > **Ref :** [Message Queue](https://blog.ntechdevelopers.com/messages-queue-cach-ma-microservice-giao-tiep-voi-nhau/)<br>
- **Mass Transit**: Open source for .NET work with various type of Message Queue use for creating distributed application system.<br>
  > **Ref :** [Mass Transit](https://masstransit.io/)<br>
  > **Ref :** [Mass Transit Spiderum](https://spiderum.com/bai-dang/Masstransit-Lam-chu-message-queue-kfCOubTdSVwy)<br>
- `await builder.Services.UseAppServiceExtension();` Using Extension for adding and configuring service into container.
  > Then go to `MassTransitServiceConfig.cs` file.
  >> `Config` method
  >>> <br>
  <dl>
    <dt>***AddConsumersFromNamespaceContaining***</dt>
    <dd>Every Consumer has a namespace or deepr namespace will be added into.</dd>
    <dt>***SetEndpointNameFormatter***</dt>
    <dd>Something like add prefix for endpoint name</dd>
    <dt>***AddConfigureEndpointsCallback***</dt>
    <dd>General configuration affects all consumers added from AddConsumersFromNamespaceContaining method above</dd>
    <dt>***ReceiveEndpoint***</dt>
    <dd>Explicit configuration for each endpoint. This is configuration calledback, etc... and use it for specific endpoints with consumer</dd>
  </dl>
- `UseMessageRetry` this will retry when consumer fail in something. <br> 

