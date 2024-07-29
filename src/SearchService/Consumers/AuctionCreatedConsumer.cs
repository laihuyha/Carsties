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
        private readonly IMapper _mapper;

        public AuctionCreatedConsumer(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
            Console.WriteLine("===> Consuming auction created: {0}", context.Message.Id);

            var item = _mapper.Map<Item>(context.Message);
            await item.SaveAsync();
        }
    }
}