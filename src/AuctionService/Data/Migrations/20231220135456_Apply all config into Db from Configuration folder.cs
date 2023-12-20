using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionService.Data.Migrations
{
    /// <inheritdoc />
    public partial class ApplyallconfigintoDbfromConfigurationfolder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Make_Model_Color_Desc",
                table: "Items",
                columns: new[] { "Make", "Model", "Color" },
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Make_Model_Year_Desc",
                table: "Items",
                columns: new[] { "Make", "Model", "Year" },
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Make_Model_Year_Mileage_Desc",
                table: "Items",
                columns: new[] { "Make", "Model", "Year", "Mileage" },
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Model_Mileage_Desc",
                table: "Items",
                columns: new[] { "Model", "Mileage" },
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Model_Year_Mileage_Desc",
                table: "Items",
                columns: new[] { "Model", "Year", "Mileage" },
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Seller_AuctionEnd_Status_All_Desc",
                table: "Auctions",
                columns: new[] { "Seller", "AuctionEnd", "Status" },
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Make_Model_Color_Desc",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Make_Model_Year_Desc",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Make_Model_Year_Mileage_Desc",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Model_Mileage_Desc",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Model_Year_Mileage_Desc",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Seller_AuctionEnd_Status_All_Desc",
                table: "Auctions");
        }
    }
}
