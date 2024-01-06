using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SearchService.Consumer;

namespace SearchService.Extensions.MassTransit
{
    public class MassTransitServiceConfig
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;

        public MassTransitServiceConfig(IServiceCollection services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        public async Task Config()
        {
            _ = _services.AddMassTransit(x =>
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
                    cfg.ReceiveEndpoint("search-auction-finished", e =>
                    {
                        e.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(10)));
                        e.ConfigureConsumer<AuctionFinishedConsumer>(context);
                    });
                    cfg.ReceiveEndpoint("search-bid-placed", e =>
                    {
                        e.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(10)));
                        e.ConfigureConsumer<BidPlacedConsumer>(context);
                    });
                    cfg.ConfigureEndpoints(context);
                    // Config for publish app can use
                    cfg.Host(_configuration["RabbitMq:Host"], "/", host =>
                    {
                        host.Username(_configuration.GetValue("RabbitMq:Username", "guest"));
                        host.Password(_configuration.GetValue("RabbitMq:Password", "guest"));
                    });
                });
            });
            await Task.CompletedTask;
        }
    }
}