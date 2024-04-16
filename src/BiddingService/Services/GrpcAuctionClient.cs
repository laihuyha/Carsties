using System;
using AuctionService;
using BiddingService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BiddingService.Services
{
    public class GrpcAuctionClient
    {
        private readonly ILogger<GrpcAuctionClient> _logger;
        private readonly IConfiguration _configuration;

        public GrpcAuctionClient(ILogger<GrpcAuctionClient> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public Auction GetAuction(string id)
        {
            _logger.LogInformation("Calling Grpc Service");
            var channel = GrpcChannel.ForAddress(_configuration["GrpcAuction"]);
            var client = new GrpcAuction.GrpcAuctionClient(channel);
            var request = new GetAuctionRequest { Id = id };

            try
            {
                var reply = client.GetAuction(request);
                var auction = new Auction
                {
                    ID = reply.Auction.Id,
                    AuctionEnd = DateTime.Parse(reply.Auction.AuctionEnd),
                    Seller = reply.Auction.Seller,
                    ReservePrice = reply.Auction.ReservePrice
                };
                return auction;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't call Grpc Server");
                return null;
            }
        }
    }
}