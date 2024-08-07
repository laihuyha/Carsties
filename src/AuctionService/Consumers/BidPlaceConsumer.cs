using System;
using System.Threading.Tasks;
using AuctionService.Data;
using Contracts.Bid;
using MassTransit;

namespace AuctionService.Consumer
{
    public class BidPlaceConsumer : IConsumer<BidPlaced>
    {
        private readonly AuctionDbContext _auctionDbContext;

        public BidPlaceConsumer(AuctionDbContext auctionDbContext)
        {
            _auctionDbContext = auctionDbContext;
        }

        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            Console.WriteLine("===> Consuming bid place BidId: {0}", context.Message.Id);
            var auction = await _auctionDbContext.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));
            if (auction.CurrentHighBid == null || context.Message.BidStatus.Contains("Accepted") && context.Message.Amount > auction.CurrentHighBid)
            {
                auction.CurrentHighBid = context.Message.Amount;
                await _auctionDbContext.SaveChangesAsync();
            }
        }
    }
}