using System;
using AuctionService.DTO;
using AuctionService.Entities;
using AutoMapper;

namespace AuctionService.Request.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Auction, AuctionDTO>().IncludeMembers(e => e.Item);
            CreateMap<Item, AuctionDTO>();
            CreateMap<CreateAuctionDTO, Auction>().ForMember(e => e.Item, opt =>
                opt.MapFrom(s => new Item
                {
                    Id = Guid.NewGuid(),
                    Color = s.Color,
                    ImageUrl = s.ImageUrl,
                    Make = s.Make,
                    Mileage = s.Mileage,
                    Model = s.Model,
                    Year = s.Year
                }
            ));
            CreateMap<CreateAuctionDTO, Item>();
        }
    }
}