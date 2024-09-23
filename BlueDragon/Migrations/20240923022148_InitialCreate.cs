using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlueDragon.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Peripherals",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Hardware",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "EComponents",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Cables",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Peripherals");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Hardware");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "EComponents");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Cables");
        }
    }
}
