using AuctionService.DTO;
using AuctionService.Entities;
using AutoMapper;
using Contracts.Auction;

namespace AuctionService.Request.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            _ = CreateMap<Auction, AuctionDto>().IncludeMembers(e => e.Item);
            _ = CreateMap<Item, AuctionDto>();
            _ = CreateMap<CreateAuctionDto, Auction>().ForMember(e => e.Item, opt => opt.MapFrom(s => s));
            _ = CreateMap<CreateAuctionDto, Item>();

            #region Mapping for using in event bus
            _ = CreateMap<AuctionDto, AuctionCreated>();
            _ = CreateMap<Auction, AuctionUpdated>().IncludeMembers(a => a.Item);
            _ = CreateMap<Item, AuctionUpdated>();
            #endregion Mapping for using in event bus
        }
    }
}