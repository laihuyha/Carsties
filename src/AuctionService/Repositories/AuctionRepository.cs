using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Data;
using AuctionService.DTO;
using AuctionService.Entities;
using AuctionService.IRepositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;
        public AuctionRepository(AuctionDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddAuction(Auction auction)
        {
            _ = _context.Auctions.Add(auction);
        }

        public void UpdateAuction(Auction auction)
        {
            _ = _context.Auctions.Update(auction);
        }

        public void RemoveAuction(Auction auction)
        {
            _ = _context.Auctions.Remove(auction);
        }

        public async Task<AuctionDto> GetAuctionByIdAsync(string id)
        {
            return await _context.Auctions.AsNoTracking().ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
        }

        public async Task<Auction> GetAuctionEntityById(string id)
        {
            return await _context.Auctions.AsNoTracking().Include(e => e.Item).FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
        }

        public async Task<List<AuctionDto>> GetAuctionsAsync(string date)
        {
            var query = _context.Auctions.AsNoTracking().OrderBy(e => e.Item.Make).AsQueryable();
            if (!string.IsNullOrEmpty(date))
            {
                // for get the newer data if it has requested from the other services
                _ = query.Where(e => e.UpdateAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) >= 0);
            }

            return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}