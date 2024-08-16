using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Polly;
using SearchService.Init;

namespace SearchService.Extensions
{
    public static class AppBuilderExtension
    {
        public static async Task<IApplicationBuilder> UseAppBuilderExtension(this IApplicationBuilder app, IConfiguration configuration)
        {
            await Policy.Handle<TimeoutException>().WaitAndRetryAsync(5, retryAttemp => TimeSpan.FromSeconds(10))
            .ExecuteAndCaptureAsync(async () =>
            {
                var mongoDbInit = new MongoDbInit(configuration);
                await mongoDbInit.Setup();
                await DbInitializer.InitDb(app.ApplicationServices);
            });
            
            return app;
        }
    }
}