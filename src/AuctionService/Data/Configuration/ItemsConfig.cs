using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionService.Data.Configuration
{
    public class ItemsConfig : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            _ = builder.HasIndex(e => new { e.Model, e.Mileage }).HasDatabaseName("IX_Model_Mileage");
            _ = builder.HasIndex(e => new { e.Model, e.Mileage }).HasDatabaseName("IX_Model_Mileage_Desc").IsDescending();

            _ = builder.HasIndex(a => new { a.Make, a.Model, a.Color }).HasDatabaseName("IX_Make_Model_Color");
            _ = builder.HasIndex(a => new { a.Make, a.Model, a.Color }).HasDatabaseName("IX_Make_Model_Color_Desc").IsDescending();

            _ = builder.HasIndex(a => new { a.Make, a.Model, a.Year }).HasDatabaseName("IX_Make_Model_Year");
            _ = builder.HasIndex(a => new { a.Make, a.Model, a.Year }).HasDatabaseName("IX_Make_Model_Year_Desc").IsDescending();

            _ = builder.HasIndex(e => new { e.Model, e.Year, e.Mileage }).HasDatabaseName("IX_Model_Year_Mileage");
            _ = builder.HasIndex(e => new { e.Model, e.Year, e.Mileage }).HasDatabaseName("IX_Model_Year_Mileage_Desc").IsDescending();

            _ = builder.HasIndex(e => new { e.Make, e.Model, e.Year, e.Mileage }).HasDatabaseName("IX_Make_Model_Year_Mileage");
            _ = builder.HasIndex(e => new { e.Make, e.Model, e.Year, e.Mileage }).HasDatabaseName("IX_Make_Model_Year_Mileage_Desc").IsDescending();
        }
    }
}