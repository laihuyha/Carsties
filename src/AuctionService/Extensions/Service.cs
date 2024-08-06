using System;
using AuctionService.Consumer;
using AuctionService.Data;
using AuctionService.IRepositories;
using AuctionService.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionService.Extensions
{
    public static class Service
    {
        public static IServiceCollection AddAppService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            AddMessageBrokerService(services, configuration);
            AddAuthService(services, configuration);
            services.AddGrpc();
            services.AddScoped<IAuctionRepository, AuctionRepository>();
            return services;
        }

        private static void AddAuthService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = configuration["IdentityServiceUrl"];
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters.ValidateAudience = false;
                opt.TokenValidationParameters.NameClaimType = "username";
            });
        }

        private static void AddMessageBrokerService(IServiceCollection services, IConfiguration configuration)
        {

            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false));
                x.AddEntityFrameworkOutbox<AuctionDbContext>(o =>
                {
                    o.QueryDelay = TimeSpan.FromSeconds(30);
                    _ = o.UsePostgres();
                    o.UseBusOutbox();
                });
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);

                    // Config for publish app can use
                    cfg.Host(configuration["RabbitMq:Host"], "/", host =>
                    {
                        host.Username(configuration.GetValue("RabbitMq:Username", "guest"));
                        host.Password(configuration.GetValue("RabbitMq:Password", "guest"));
                    });
                });
            });
        }
    }
}