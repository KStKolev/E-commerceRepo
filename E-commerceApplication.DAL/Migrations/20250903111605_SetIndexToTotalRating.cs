using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerceApplication.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SetIndexToTotalRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_TotalRating",
                table: "Products",
                column: "TotalRating");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_TotalRating",
                table: "Products");
        }
    }
}
