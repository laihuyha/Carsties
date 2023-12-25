using System;
using System.Threading.Tasks;
using Contracts.Auction;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumer
{
    public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
    {

        public AuctionDeletedConsumer()
        {
        }

        public async Task Consume(ConsumeContext<AuctionDeleted> context)
        {
            Console.Write("===> Consuming auction deleted: {0}", context.Message.Id);
            var item = await DB.Find<Item>().OneAsync(context.Message.Id);
            if (item != null)
            {
                _ = await item.DeleteAsync();
            }
            else
            {
                Console.Write("===> Auction deleted fault");
                throw new Exception("Item not found");
            }
        }
    }
}