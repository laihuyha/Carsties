using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionService.DTO;
using AuctionService.Entities;
using AuctionService.IRepositories;
using AutoMapper;
using Contracts.Auction;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionService.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    public class AuctionsController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IAuctionRepository _auctionRepository;
        private readonly IMapper _mapper;

        public AuctionsController(IAuctionRepository auctionRepository, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _mapper = mapper;
            _auctionRepository = auctionRepository;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<AuctionDto>>> GetAuctions(string date)
        {
            return await _auctionRepository.GetAuctionsAsync(date);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> Get(string Id)
        {
            var auction = await _auctionRepository.GetAuctionByIdAsync(Id);
            return auction == null ? (ActionResult<AuctionDto>)NotFound() : (ActionResult<AuctionDto>)auction;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctiondto)
        {
            try
            {
                var auction = _mapper.Map<Auction>(auctiondto);
                auction.Seller = User.Identity.Name;

                _auctionRepository.AddAuction(auction);

                var newAuction = _mapper.Map<AuctionDto>(auction);
                await _publishEndpoint.Publish(_mapper.Map<AuctionCreated>(newAuction));

                await _auctionRepository.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = auction.Id }, newAuction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<AuctionDto>> UpdateAuction(Guid id, UpdateAuctionDto auctiondto)
        {
            try
            {
                var auction = await _auctionRepository.GetAuctionEntityById(id.ToString());
                if (auction == null) return NotFound();

                if (auction.Seller != User.Identity.Name) return Forbid();

                auction.Item.Make = auctiondto.Make;
                auction.Item.Model = auctiondto.Model;
                auction.Item.Year = auctiondto.Year;
                auction.Item.Color = auctiondto.Color;
                auction.Item.Mileage = auctiondto.Mileage;

                _auctionRepository.UpdateAuction(auction);

                await _publishEndpoint.Publish(_mapper.Map<AuctionUpdated>(auction.Item));

                var result = await _auctionRepository.SaveChangesAsync();

                return result ? Ok(_mapper.Map<AuctionDto>(auction.Item)) : BadRequest("Error while updating");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var auction = await _auctionRepository.GetAuctionEntityById(id.ToString());
                if (auction == null) return NotFound();

                if (auction.Seller != User.Identity.Name) return Forbid();

                _auctionRepository.RemoveAuction(auction);

                await _publishEndpoint.Publish(new AuctionDeleted { Id = auction.Id.ToString() });

                var result = await _auctionRepository.SaveChangesAsync();
                return result ? Ok() : BadRequest("Error while deleting");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}