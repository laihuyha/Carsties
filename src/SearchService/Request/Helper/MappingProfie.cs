using AutoMapper;
using Contracts.Auction;
using SearchService.Models;

namespace SearchService.Request.Helper
{
    public class MappingProfie : Profile
    {
        public MappingProfie()
        {
            _ = CreateMap<AuctionCreated, Item>().ForMember(e => e.ID, opt => opt.MapFrom(x => x.Id));
            _ = CreateMap<AuctionUpdated, Item>().ForMember(e => e.ID, opt => opt.MapFrom(x => x.Id));
        }
    }
}