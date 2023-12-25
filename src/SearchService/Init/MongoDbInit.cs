using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Init
{
    public class MongoDbInit
    {
        private readonly IConfiguration configuration;
        public MongoDbInit(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task Setup()
        {
            await DB.InitAsync("SearchDb", MongoClientSettings.FromConnectionString(configuration.GetConnectionString("MongoDb")));

            await IndexesModels();
        }

        private static async Task IndexesModels()
        {
            _ = await DB.Index<Item>()
                .Key(x => x.Make, KeyType.Text)
                .Key(x => x.Model, KeyType.Text)
                .Key(x => x.Color, KeyType.Text)
                .CreateAsync();
        }
    }
}