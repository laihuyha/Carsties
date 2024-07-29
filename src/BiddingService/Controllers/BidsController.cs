using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BiddingService.DTOs;
using BiddingService.Models;
using BiddingService.Services;
using Contracts.Bid;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace BiddingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly GrpcAuctionClient _grpcAuctionClient;

        public BidsController(IMapper mapper, IPublishEndpoint publishEndpoint, GrpcAuctionClient grpcAuctionClient)
        {
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _grpcAuctionClient = grpcAuctionClient;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BidDto>> PlaceBid(string auctionId, int amount)
        {
            var auction = await GetAuction(auctionId);

            if (auction == null)
            {
                return NotFound();
            }

            if (IsUserSeller(auction))
            {
                return BadRequest("You cannot bid on your own auction.");
            }

            var bid = CreateBid(auctionId, amount);

            await SetBidStatus(bid, auction);

            await SaveBid(bid);

            await PublishBidPlaceEvent(bid);

            return Ok(_mapper.Map<BidDto>(bid));
        }

        [HttpGet("{auctionId}")]
        public async Task<ActionResult<List<BidDto>>> GetBidForAuction(string auctionId)
        {
            var bids = await DB.Find<Bid>()
            .Match(a => a.AuctionId == auctionId)
            .Sort(b => b.Descending(e => e.BidTime)).ExecuteAsync();

            return bids.Select(_mapper.Map<Bid, BidDto>).ToList();
        }

        #region Private method
        private async Task<Auction> GetAuction(string auctionId)
        {
            var auction = await DB.Find<Auction>().OneAsync(auctionId);

            auction ??= _grpcAuctionClient.GetAuction(auctionId);

            return auction;
        }

        private bool IsUserSeller(Auction auction)
        {
            return auction.Seller == User.Identity.Name;
        }

        private Bid CreateBid(string auctionId, int amount)
        {
            return new Bid
            {
                Amount = amount,
                AuctionId = auctionId,
                Bidder = User.Identity.Name,
            };
        }

        private async Task SetBidStatus(Bid bid, Auction auction)
        {
            if (auction.AuctionEnd < DateTime.UtcNow)
            {
                bid.BidStatus = BidStatus.Finished;
            }
            else
            {
                var highBid = await GetHighestBid(auction.ID);

                if (highBid != null && bid.Amount > highBid.Amount || highBid == null)
                {
                    bid.BidStatus = bid.Amount > auction.ReservePrice ? BidStatus.Accepted : BidStatus.AcceptedBelowReserve;
                }

                if (highBid != null && bid.Amount <= highBid.Amount)
                {
                    bid.BidStatus = BidStatus.TooLow;
                }
            }
        }

        private async Task<Bid> GetHighestBid(string auctionId)
        {
            return await DB.Find<Bid>().Match(a => a.AuctionId == auctionId).Sort(e => e.Descending(x => x.Amount)).ExecuteFirstAsync();
        }

        private async Task SaveBid(Bid bid)
        {
            await DB.SaveAsync(bid);
        }

        private async Task PublishBidPlaceEvent(Bid bid)
        {
            await _publishEndpoint.Publish(_mapper.Map<BidPlaced>(bid));
        }
        #endregion Private method
    }
}