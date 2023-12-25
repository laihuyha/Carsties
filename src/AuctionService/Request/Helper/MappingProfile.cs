using System;
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
            _ = CreateMap<Auction, AuctionDTO>().IncludeMembers(e => e.Item);
            _ = CreateMap<Item, AuctionDTO>();
            _ = CreateMap<CreateAuctionDTO, Auction>().ForMember(e => e.Id, opt => opt.MapFrom(x => Guid.NewGuid())).ForMember(e => e.Item, opt => opt.MapFrom(s => s));
            _ = CreateMap<CreateAuctionDTO, Item>();

            #region Mapping for using in event bus
            _ = CreateMap<AuctionDTO, AuctionCreated>();
            _ = CreateMap<Item, AuctionUpdated>().ForMember(e => e.Id, opt => opt.MapFrom(x => x.AuctionId));
            #endregion Mapping for using in event bus
        }
    }
}