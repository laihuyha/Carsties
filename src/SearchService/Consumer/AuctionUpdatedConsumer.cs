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
        private readonly IMapper mapper;

        public AuctionUpdatedConsumer(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<AuctionUpdated> context)
        {
            Console.WriteLine("===> Consuming auction updated: {0}", context.Message.Id);
            var item = await DB.Find<Item>().OneAsync(context.Message.Id);
            if (item != null)
            {
                await DB.Update<Item>().MatchID(context.Message.Id)
                    .Modify(x => x.Make, context.Message.Make)
                    .Modify(x => x.Model, context.Message.Model)
                    .Modify(x => x.Year, context.Message.Year)
                    .Modify(x => x.Color, context.Message.Color)
                    .Modify(x => x.Mileage, context.Message.Mileage)
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