using AutoMapper;
using BiddingService.DTOs;
using BiddingService.Models;
using Contracts.Bid;

namespace BiddingService.Request.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Bid, BidDto>();
            CreateMap<Bid, BidPlace>();
        }
    }
}