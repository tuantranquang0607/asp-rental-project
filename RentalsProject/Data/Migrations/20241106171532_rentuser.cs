using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalsProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class rentuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "RentalListings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentalListings_UserId",
                table: "RentalListings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalListings_AspNetUsers_UserId",
                table: "RentalListings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalListings_AspNetUsers_UserId",
                table: "RentalListings");

            migrationBuilder.DropIndex(
                name: "IX_RentalListings_UserId",
                table: "RentalListings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RentalListings");
        }
    }
}
