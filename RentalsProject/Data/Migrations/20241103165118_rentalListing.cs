using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalsProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class rentalListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentalListings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListingSummary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListingDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListingAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CivicNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListingPricePerDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumberOfBedrooms = table.Column<int>(type: "int", nullable: false),
                    NumberOfBathrooms = table.Column<int>(type: "int", nullable: false),
                    SizeInSquareFeet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalListings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentalListings");
        }
    }
}
