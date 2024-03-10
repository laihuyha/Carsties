using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Data;
using AuctionService.DTO;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts.Auction;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    public class AuctionsController : ControllerBase
    {
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public AuctionsController(AuctionDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<AuctionDTO>>> GetAuctions(string date)
        {
            var query = _context.Auctions.AsNoTracking().OrderBy(e => e.Item.Make).AsQueryable();
            if (!string.IsNullOrEmpty(date))
            {
                // for get the newer data if it has requested from the other services
                _ = query.Where(e => e.UpdateAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) >= 0);
            }

            return await query.ProjectTo<AuctionDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDTO>> Get(string Id)
        {
            var auction = await _context.Auctions
                            .Include(e => e.Item).AsNoTracking().AsSplitQuery()
                            .SingleOrDefaultAsync(e => e.Id.ToString() == Id);
            return auction == null ? (ActionResult<AuctionDTO>)NotFound() : (ActionResult<AuctionDTO>)_mapper.Map<AuctionDTO>(auction);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<AuctionDTO>> CreateAuction(CreateAuctionDTO auctiondto)
        {
            try
            {
                var auction = _mapper.Map<Auction>(auctiondto);
                auction.Seller = User.Identity.Name;
                _ = _context.Auctions.Add(auction);

                var newAuction = _mapper.Map<AuctionDTO>(auction);
                await _publishEndpoint.Publish(_mapper.Map<AuctionCreated>(newAuction));

                _ = await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = auction.Id }, newAuction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<AuctionDTO>> UpdateAuction(Guid id, UpdateAuctionDTO auctiondto)
        {
            try
            {
                var auction = await _context.Auctions.Include(e => e.Item).SingleOrDefaultAsync(e => e.Id == id);
                if (auction == null) return NotFound();

                if (auction.Seller != User.Identity.Name) return Forbid();

                auction.Item.Make = auctiondto.Make;
                auction.Item.Model = auctiondto.Model;
                auction.Item.Year = auctiondto.Year;
                auction.Item.Color = auctiondto.Color;
                auction.Item.Mileage = auctiondto.Mileage;

                _ = _context.Auctions.Update(auction);
                await _publishEndpoint.Publish(_mapper.Map<AuctionUpdated>(auction.Item));

                var result = await _context.SaveChangesAsync();

                return result > 0 ? Ok(_mapper.Map<AuctionDTO>(auction.Item)) : BadRequest("Error while updating");
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
                var auction = await _context.Auctions.FindAsync(id);
                if (auction == null) return NotFound();

                if (auction.Seller != User.Identity.Name) return Forbid();
                _ = _context.Auctions.Remove(auction);
                await _publishEndpoint.Publish(new AuctionDeleted { Id = auction.Id.ToString() });
                var result = await _context.SaveChangesAsync();
                return result > 0 ? Ok() : BadRequest("Error while deleting");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}