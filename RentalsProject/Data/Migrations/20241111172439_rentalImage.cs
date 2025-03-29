using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalsProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class rentalImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RentalImageId",
                table: "RentalListings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentalListings_RentalImageId",
                table: "RentalListings",
                column: "RentalImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalListings_Files_RentalImageId",
                table: "RentalListings",
                column: "RentalImageId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalListings_Files_RentalImageId",
                table: "RentalListings");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropIndex(
                name: "IX_RentalListings_RentalImageId",
                table: "RentalListings");

            migrationBuilder.DropColumn(
                name: "RentalImageId",
                table: "RentalListings");
        }
    }
}
