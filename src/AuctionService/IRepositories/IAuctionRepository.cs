using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionService.DTO;
using AuctionService.Entities;

namespace AuctionService.IRepositories
{
    public interface IAuctionRepository
    {
        Task<List<AuctionDto>> GetAuctionsAsync(string date);
        Task<AuctionDto> GetAuctionByIdAsync(string id);
        Task<Auction> GetAuctionEntityById(string id);
        void AddAuction(Auction auction);
        void UpdateAuction(Auction auction);
        void RemoveAuction(Auction auction);
        Task<bool> SaveChangesAsync();
    }
}