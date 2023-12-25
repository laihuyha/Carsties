using System;
using System.Net;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;

namespace SearchService.Services.Policy.Handler
{
    public static class Resilient
    {

        public static IAsyncPolicy<HttpResponseMessage> GetPolicy()
        {
            return HttpPolicyExtensions.HandleTransientHttpError().OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
            .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));
        }
    }
}