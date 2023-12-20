using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Data;
using AuctionService.DTO;
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
    }
}