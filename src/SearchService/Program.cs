using System;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SearchService.Consumer;
using SearchService.Extensions;
using SearchService.Services;
using SearchService.Services.Policy.Handler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionServiceHttpClient>().AddPolicyHandler(Resilient.GetPolicy());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));
    x.AddConfigureEndpointsCallback((name, cfg) => { cfg.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(10))); });
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ReceiveEndpoint("search-auction-created", e =>
        {
            e.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(10)));
            e.ConfigureConsumer<AuctionCreatedConsumer>(context);
        });
        cfg.ReceiveEndpoint("search-auction-updated", e =>
        {
            e.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(10)));
            e.ConfigureConsumer<AuctionUpdatedConsumer>(context);
        });
        cfg.ReceiveEndpoint("search-auction-deleted", e =>
        {
            e.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(10)));
            e.ConfigureConsumer<AuctionDeletedConsumer>(context);
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#region Add Extensions

// Config the lifetime of the application
app.Lifetime.ApplicationStarted.Register(async () =>
{
    _ = await app.UseAppBuilderExtension(builder.Configuration);
});

#endregion

app.Run();