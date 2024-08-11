using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Controllers;
using AuctionService.DTO;
using AuctionService.Entities;
using AuctionService.IRepositories;
using AuctionService.Request.Helper;
using AutoFixture;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
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

            _controller = new AuctionsController(_auctionRepositoryMock.Object, _mapper, _publishEndpointMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = Helpers.Helpers.GetClaimsPrincipal() }
                }
            };
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

        [Fact]
        public async Task GetAuctionById_WithValidGuid_ReturnsAuction()
        {
            //arrange
            var auction = _fixture.Create<AuctionDto>();

            _auctionRepositoryMock.Setup(repo => repo.GetAuctionByIdAsync(It.IsAny<string>())).ReturnsAsync(auction);

            //act
            var result = await _controller.Get(auction.Id.ToString());

            //assert
            Assert.Equal(auction.Make, result.Value.Make);
            Assert.IsType<ActionResult<AuctionDto>>(result);
        }

        [Fact]
        public async Task GetAuctionById_WithInValidGuid_ReturnsNotFound()
        {
            //arrange
            _auctionRepositoryMock.Setup(repo => repo.GetAuctionByIdAsync(It.IsAny<string>())).ReturnsAsync(value: null);

            //act
            var result = await _controller.Get(Guid.NewGuid().ToString());

            //assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateAuction_WithValidCreateAuctionDto_ReturnsCreatedAtAction()
        {
            //arrange
            var auction = _fixture.Create<CreateAuctionDto>();
            _auctionRepositoryMock.Setup(repo => repo.AddAuction(It.IsAny<Auction>()));
            _auctionRepositoryMock.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(true);

            //act
            var result = await _controller.CreateAuction(auction);
            var createdResult = result.Result as CreatedAtActionResult;

            //assert
            Assert.NotNull(createdResult);
            Assert.Equal("Get", createdResult.ActionName);
            Assert.IsType<AuctionDto>(createdResult.Value);
        }
    }
}