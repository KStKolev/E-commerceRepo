using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_commerceApplication.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedProductTestDataMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "DateCreated", "Name", "Platform", "Price", "TotalRating" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Halo Infinite", 2, 59.99m, 8.5 },
                    { 2, new DateTime(2018, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "God of War", 1, 39.99m, 9.8000000000000007 },
                    { 3, new DateTime(2020, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Half-Life: Alyx", 2, 49.99m, 9.1999999999999993 },
                    { 4, new DateTime(2017, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Legend of Zelda: Breath of the Wild", 0, 59.99m, 9.6999999999999993 },
                    { 5, new DateTime(2022, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Elden Ring", 3, 69.99m, 9.5999999999999996 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
