using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SearchService.Extensions.MassTransit;

namespace SearchService.Extensions
{
    public static class AppServiceExtension
    {
        public static async Task<IServiceCollection> UseAppServiceExtension(this IServiceCollection services, IConfiguration configuration)
        {
            var massTransitCfg = new MassTransitServiceConfig(services, configuration);
            await massTransitCfg.Config();

            return services;
        }
    }
}