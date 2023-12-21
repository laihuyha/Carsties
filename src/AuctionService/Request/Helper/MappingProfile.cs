using AuctionService.DTO;
using AuctionService.Entities;
using AutoMapper;

namespace AuctionService.Request.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            _ = CreateMap<Auction, AuctionDTO>().IncludeMembers(e => e.Item);
            _ = CreateMap<Item, AuctionDTO>();
            _ = CreateMap<CreateAuctionDTO, Auction>().ForMember(e => e.Item, opt => opt.MapFrom(s => s));
            _ = CreateMap<CreateAuctionDTO, Item>();
        }
    }
}