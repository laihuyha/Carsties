using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Data;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService
{
    [ApiController]
    [Route("api/auctions")]
    public class AuctionsController : ControllerBase
    {
        private readonly AuctionDbContext _auctionDbContext;
        private readonly IMapper _mapper;
        public AuctionsController(AuctionDbContext auctionDbContext, IMapper mapper)
        {
            _auctionDbContext = auctionDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Auction>>> Get()
        {
            var auctions = _auctionDbContext.Auctions.Include(e => e.Item).AsNoTracking().OrderByDescending(x => x.Item.Year).AsSplitQuery();
            return await auctions.ToListAsync();
        }
    }
}