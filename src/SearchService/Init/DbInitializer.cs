using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Init
{
    public class DbInitializer
    {
        public static async Task InitDb()
        {
            var count = await DB.CountAsync<Item>();
            if (count == 0)
            {
                Console.WriteLine("No items in DB. Seeding...");
                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", "auctions.json");
                var data = await File.ReadAllTextAsync(path);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var items = JsonSerializer.Deserialize<Item[]>(data, options);
                _ = await DB.SaveAsync(items);
            }
        }
    }
}