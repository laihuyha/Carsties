using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SearchService.Extensions.MassTransit;

namespace SearchService.Extensions
{
    public static class AppServiceExtension
    {
        public static async Task<IServiceCollection> UseAppServiceExtension(this IServiceCollection services)
        {
            var massTransitCfg = new MassTransitServiceConfig(services);
            await massTransitCfg.Config();

            return services;
        }
    }
}