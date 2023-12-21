using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using SearchService.Data;
using SearchService.Init;

namespace SearchService.Extensions
{
    public static class AppBuilderExtension
    {
        public static async Task<IApplicationBuilder> UseAppBuilderExtension(this IApplicationBuilder app, IConfiguration configuration)
        {
            try
            {
                var mongoDbInit = new MongoDbInit(configuration);
                await mongoDbInit.Init();
                await DbInitializer.InitDb();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return app;
        }
    }
}