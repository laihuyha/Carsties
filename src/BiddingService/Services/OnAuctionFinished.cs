using System;
using System.Threading;
using System.Threading.Tasks;
using BiddingService.Models;
using Contracts.Auction;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Entities;

namespace BiddingService.Services
{
    public class OnAuctionFinished : BackgroundService
    {
        private readonly ILogger<OnAuctionFinished> _logger;
        private readonly IServiceProvider _serviceProvider;
        private AuctionFinished AuctionFinished { get; set; }

        public OnAuctionFinished(ILogger<OnAuctionFinished> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            AuctionFinished = new AuctionFinished();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting check for finishing auctions");

            stoppingToken.Register(() => _logger.LogInformation("==> Auction check is stopping"));

            while (!stoppingToken.IsCancellationRequested)
            {
                await OnFinished(stoppingToken);

                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task OnFinished(CancellationToken stoppingToken)
        {
            var finishedAuctions = await DB.Find<Auction>()
            .Match(x => x.AuctionEnd <= DateTime.UtcNow)
            .Match(x => !x.Finished)
            .ExecuteAsync(stoppingToken);

            if (finishedAuctions.Count == 0) return;

            _logger.LogInformation($"==> Found {finishedAuctions.Count}");

            using var scope = _serviceProvider.CreateScope();
            var endPoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            foreach (var auction in finishedAuctions)
            {
                auction.Finished = true;
                await auction.SaveAsync(null, stoppingToken);

                var winningBid = await DB.Find<Bid>().Match(a => a.AuctionId == auction.ID)
                .Match(e => e.BidStatus == BidStatus.Accepted)
                .Sort(x => x.Descending(s => s.Amount))
                .ExecuteFirstAsync(stoppingToken);

                UpdateAuctionFinishedEntity(winningBid, auction);

                await endPoint.Publish(AuctionFinished, stoppingToken);
            }
        }

        private void UpdateAuctionFinishedEntity(Bid winningBid, Auction auction)
        {
            AuctionFinished.ItemSold = winningBid != null;
            AuctionFinished.AuctionId = auction.ID;
            AuctionFinished.Winner = winningBid?.Bidder;
            AuctionFinished.Amount = winningBid?.Amount;
            AuctionFinished.Seller = auction.Seller;
        }
    }
}