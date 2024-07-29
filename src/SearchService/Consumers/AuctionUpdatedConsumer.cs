using System;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.Auction;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumer
{
    public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
    {
        public async Task Consume(ConsumeContext<AuctionUpdated> context)
        {
            Console.WriteLine("===> Consuming auction updated: {0}", context.Message.Id);
            var item = await DB.Find<Item>().OneAsync(context.Message.Id);
            if (item != null)
            {
                _ = await DB.Update<Item>().MatchID(context.Message.Id)
                .ModifyOnly(x => new { x.Make, x.Model, x.Color, x.Year, x.Mileage }, item)
                .ExecuteAsync();
            }
            else
            {
                Console.Write("===> Auction updated fault");
                throw new Exception("Item not found");
            }
        }
    }
}