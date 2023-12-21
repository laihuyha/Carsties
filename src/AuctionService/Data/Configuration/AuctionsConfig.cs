using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionService.Data.Configuration
{
    public class AuctionsConfig : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            _ = builder.HasIndex(a => new { a.Seller, a.AuctionEnd, a.Status })
            .HasDatabaseName("IX_Seller_AuctionEnd_Status_All_Asc");

            _ = builder.HasIndex(a => new { a.Seller, a.AuctionEnd, a.Status })
            .HasDatabaseName("IX_Seller_AuctionEnd_Status_All_Desc")
            .IsDescending(); //======> Introduce from EF.Core 7.0
        }
    }
}