using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnusPunch.Data.Migrations
{
    /// <inheritdoc />
    public partial class SnusDataEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SnusName",
                table: "tblEntry",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SnusPortionNicotineInMg",
                table: "tblEntry",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SnusPortionPriceInSek",
                table: "tblEntry",
                type: "decimal(6,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SnusName",
                table: "tblEntry");

            migrationBuilder.DropColumn(
                name: "SnusPortionNicotineInMg",
                table: "tblEntry");

            migrationBuilder.DropColumn(
                name: "SnusPortionPriceInSek",
                table: "tblEntry");
        }
    }
}
