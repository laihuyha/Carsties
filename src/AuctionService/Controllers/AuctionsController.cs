using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Data;
using AuctionService.DTO;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    public class AuctionsController : ControllerBase
    {
        private readonly AuctionDbContext context;
        private readonly IMapper mapper;

        public AuctionsController(AuctionDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<AuctionDTO>>> GetAuctions()
        {
            var auctions = context.Auctions.Include(e => e.Item).AsNoTracking().OrderByDescending(e => e.Item.Year).AsSplitQuery();
            return mapper.Map<List<AuctionDTO>>(await auctions.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDTO>> Get(string Id)
        {
            var auction = await context.Auctions
                            .Include(e => e.Item).AsNoTracking().AsSplitQuery()
                            .SingleOrDefaultAsync(e => e.Id.ToString() == Id);
            return auction == null ? (ActionResult<AuctionDTO>)NotFound() : (ActionResult<AuctionDTO>)mapper.Map<AuctionDTO>(auction);
        }

        [HttpPost]
        public async Task<ActionResult<AuctionDTO>> CreateAuction(CreateAuctionDTO auctiondto)
        {
            try
            {
                var auction = mapper.Map<Auction>(auctiondto);
                auction.Seller = "";
                _ = context.Auctions.Add(auction);
                _ = await context.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = auction.Id }, mapper.Map<AuctionDTO>(auction));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AuctionDTO>> UpdateAuction(Guid Id, UpdateAuctionDTO auctiondto)
        {
            try
            {
                var auction = await context.Auctions.Include(e => e.Item).SingleOrDefaultAsync(e => e.Id == Id);
                if (auction == null) return NotFound();

                auction.Item.Make = auctiondto.Make;
                auction.Item.Model = auctiondto.Model;
                auction.Item.Year = auctiondto.Year;
                auction.Item.Color = auctiondto.Color;
                auction.Item.Mileage = auctiondto.Mileage;

                _ = context.Auctions.Update(auction);
                var result = await context.SaveChangesAsync();

                return result > 0 ? Ok() : BadRequest("Error while updating");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid Id)
        {
            try
            {
                var auction = await context.Auctions.FindAsync(Id);
                if (auction == null) return NotFound();

                if (auction.Seller == "") // Check if current user is the seller of the auction
                {
                    _ = context.Auctions.Remove(auction);
                }
                var result = await context.SaveChangesAsync();
                return result > 0 ? Ok() : BadRequest("Error while deleting");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}