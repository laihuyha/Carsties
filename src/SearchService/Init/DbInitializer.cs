using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Entities;
using SearchService.Services;

namespace SearchService.Init
{
    public static class DbInitializer
    {
        public static async Task InitDb(IServiceProvider serviceProvider)
        {

            #region Read from file
            // var count = await DB.CountAsync<Item>();
            // if (count == 0)
            // {
            //     Console.WriteLine("No items in DB. Seeding...");
            //     var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", "auctions.json");
            //     var data = await File.ReadAllTextAsync(path);
            //     var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            //     var items = JsonSerializer.Deserialize<Item[]>(data, options);
            //     _ = await DB.SaveAsync(items);
            // }
            #endregion

            #region Get from auction service
            using var scope = serviceProvider.CreateScope();
            var httpClient = scope.ServiceProvider.GetRequiredService<AuctionServiceHttpClient>();
            var items = await httpClient.GetItemsAsync();
            if (items.Count > 0) _ = await DB.SaveAsync(items);
            #endregion
        }
    }
}