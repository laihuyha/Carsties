using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using SearchService.Consumer;

namespace SearchService.Extensions.MassTransit
{
    public class MassTransitServiceConfig
    {
        private readonly IServiceCollection services;

        public MassTransitServiceConfig(IServiceCollection services)
        {
            this.services = services;
        }

        public async Task Config()
        {
            _ = services.AddMassTransit(x =>
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
            await Task.CompletedTask;
        }
    }
}