using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnusPunch.Data.Migrations
{
    /// <inheritdoc />
    public partial class Replies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentCommentId",
                table: "tblEntryComment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblEntryComment_ParentCommentId",
                table: "tblEntryComment",
                column: "ParentCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblEntryComment_tblEntryComment_ParentCommentId",
                table: "tblEntryComment",
                column: "ParentCommentId",
                principalTable: "tblEntryComment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblEntryComment_tblEntryComment_ParentCommentId",
                table: "tblEntryComment");

            migrationBuilder.DropIndex(
                name: "IX_tblEntryComment_ParentCommentId",
                table: "tblEntryComment");

            migrationBuilder.DropColumn(
                name: "ParentCommentId",
                table: "tblEntryComment");
        }
    }
}
