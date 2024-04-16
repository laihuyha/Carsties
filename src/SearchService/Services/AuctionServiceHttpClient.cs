using System;
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
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public AuctionServiceHttpClient(HttpClient client, IConfiguration configuration)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<List<Item>> GetItemsAsync()
        {
            try
            {
                var lastUpdateTimestamp = await DB.Find<Item, string>()
                    .Sort(e => e.Descending(x => x.UpdateAt))
                    .Project(a => a.UpdateAt.ToString())
                    .ExecuteAsync();

                var apiUrl = $"{_configuration["AuctionServiceUrl"]}/api/auctions/all?date={lastUpdateTimestamp}";

                var items = await _client.GetFromJsonAsync<List<Item>>(apiUrl);

                Console.WriteLine("Getting {0} items from auction service", items.Count);

                return items;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error when getting items: {ex.Message}");
                return [];
            }
        }
    }
}
