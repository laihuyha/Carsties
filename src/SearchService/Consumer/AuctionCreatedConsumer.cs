using System;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.Auction;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumer
{
    public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
    {
        private readonly IMapper mapper;

        public AuctionCreatedConsumer(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
            Console.WriteLine("===> Consuming auction created: {0}", context.Message.Id);

            var item = mapper.Map<Item>(context.Message);
            await item.SaveAsync();
        }
    }
}