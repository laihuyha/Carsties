using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Controllers;
using AuctionService.DTO;
using AuctionService.IRepositories;
using AuctionService.Request.Helper;
using AutoFixture;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AuctionService.UnitTest
{
    public class AuctionControllerTests
    {
        private readonly Mock<IAuctionRepository> _auctionRepositoryMock;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly Fixture _fixture;
        private readonly AuctionsController _controller;
        private readonly IMapper _mapper;

        public AuctionControllerTests()
        {
            _fixture = new Fixture();
            _auctionRepositoryMock = new Mock<IAuctionRepository>();
            _publishEndpointMock = new Mock<IPublishEndpoint>();

            var mockMapper = new MapperConfiguration(mc => mc.AddMaps(typeof(MappingProfile).Assembly)).CreateMapper();

            _mapper = new Mapper(mockMapper.ConfigurationProvider);

            _controller = new AuctionsController(_auctionRepositoryMock.Object, _mapper, _publishEndpointMock.Object);
        }

        [Fact]
        public async Task GetAuctions_WithNoParams_Returns10Auctions()
        {
            //arrange
            var auctions = _fixture.CreateMany<AuctionDto>(10).ToList();

            _auctionRepositoryMock.Setup(repo => repo.GetAuctionsAsync(null)).ReturnsAsync(auctions);

            //act
            var result = await _controller.GetAuctions(null);

            //assert
            Assert.Equal(10, result.Value.Count);
            Assert.IsType<ActionResult<List<AuctionDto>>>(result);
        }
    }
}