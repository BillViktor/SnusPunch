using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnusPunch.Data.Migrations
{
    /// <inheritdoc />
    public partial class Cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblEntryComment_tblEntry_EntryId",
                table: "tblEntryComment");

            migrationBuilder.DropForeignKey(
                name: "FK_tblEntryLike_tblEntry_EntryId",
                table: "tblEntryLike");

            migrationBuilder.AddForeignKey(
                name: "FK_tblEntryComment_tblEntry_EntryId",
                table: "tblEntryComment",
                column: "EntryId",
                principalTable: "tblEntry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblEntryLike_tblEntry_EntryId",
                table: "tblEntryLike",
                column: "EntryId",
                principalTable: "tblEntry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblEntryComment_tblEntry_EntryId",
                table: "tblEntryComment");

            migrationBuilder.DropForeignKey(
                name: "FK_tblEntryLike_tblEntry_EntryId",
                table: "tblEntryLike");

            migrationBuilder.AddForeignKey(
                name: "FK_tblEntryComment_tblEntry_EntryId",
                table: "tblEntryComment",
                column: "EntryId",
                principalTable: "tblEntry",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblEntryLike_tblEntry_EntryId",
                table: "tblEntryLike",
                column: "EntryId",
                principalTable: "tblEntry",
                principalColumn: "Id");
        }
    }
}
