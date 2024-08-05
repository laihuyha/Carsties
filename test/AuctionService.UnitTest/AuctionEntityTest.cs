using System;
using AuctionService.Entities;

namespace AuctionService.UnitTest;

public class AuctionEntityTest
{
    [Fact]
    public void HasReservePrice_ReservepriceGreaterThanZero_True()
    {
        var auction = new Auction { Id = Guid.NewGuid(), ReservePrice = 10 };

        var result = auction.HasReservePrice();

        Assert.True(result);
    }

    [Fact]
    public void HasReservePrice_ReservepriceIsZero_False()
    {
        var auction = new Auction { Id = Guid.NewGuid(), ReservePrice = 0 };

        var result = auction.HasReservePrice();

        Assert.False(result);
    }
}