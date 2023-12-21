using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using SearchService.Init;

namespace SearchService.Extensions
{
    public static class AppBuilderExtension
    {
        public static async Task<IApplicationBuilder> UseAppBuilderExtension(this IApplicationBuilder app, IConfiguration configuration)
        {
            var mongoDbInit = new MongoDbInit(configuration);
            await mongoDbInit.Init();

            return app;
        }
    }
}