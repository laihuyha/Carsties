using System;
using System.Threading.Tasks;
using AuctionService.Data;
using AuctionService.Entities;
using Contracts.Auction;
using MassTransit;

namespace AuctionService.Consumer
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        private readonly AuctionDbContext _auctionDbContext;

        public AuctionFinishedConsumer(AuctionDbContext auctionDbContext)
        {
            _auctionDbContext = auctionDbContext;
        }

        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            Console.WriteLine("===> Consuming auction finished: {0}", context.Message.AuctionId);
            var auction = await _auctionDbContext.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));
            if (context.Message.ItemSold)
            {
                auction.Winner = context.Message.Winner;
                auction.SoldAmount = context.Message.Amount;
            }
            auction.Status = auction.SoldAmount > auction.ReservePrice ? Status.Finished : Status.ReserveNotMet;
            await _auctionDbContext.SaveChangesAsync();
        }
    }
}