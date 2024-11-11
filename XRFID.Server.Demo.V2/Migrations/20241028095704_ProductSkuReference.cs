using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XRFID.Server.Demo.V2.Migrations
{
    /// <inheritdoc />
    public partial class ProductSkuReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "Readers");

            migrationBuilder.AddColumn<string>(
                name: "SkuReference",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SkuReference",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "Readers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
