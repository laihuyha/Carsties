using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services
{
    public class AuctionServiceHttpClient
    {
        private readonly HttpClient client;
        private readonly IConfiguration configuration;

        public AuctionServiceHttpClient(HttpClient client, IConfiguration configuration)
        {
            this.client = client;
            this.configuration = configuration;
        }

        public async Task<List<Item>> GetItemsAsync()
        {
            var LastUpdate = await DB.Find<Item, string>()
                .Sort(e => e.Descending(x => x.UpdateAt))
                .Project(a => a.UpdateAt.ToString())
                .ExecuteAsync();

            return await client.GetFromJsonAsync<List<Item>>(configuration["AuctionServiceUrl"] + "/api/auctions/all?date=" + LastUpdate);
        }
    }
}