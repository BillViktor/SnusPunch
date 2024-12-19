using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnusPunch.Data.Migrations
{
    /// <inheritdoc />
    public partial class CommentReply : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SnusPunchUserModelIdRepliedTo",
                table: "tblEntryComment",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblEntryComment_SnusPunchUserModelIdRepliedTo",
                table: "tblEntryComment",
                column: "SnusPunchUserModelIdRepliedTo");

            migrationBuilder.AddForeignKey(
                name: "FK_tblEntryComment_AspNetUsers_SnusPunchUserModelIdRepliedTo",
                table: "tblEntryComment",
                column: "SnusPunchUserModelIdRepliedTo",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblEntryComment_AspNetUsers_SnusPunchUserModelIdRepliedTo",
                table: "tblEntryComment");

            migrationBuilder.DropIndex(
                name: "IX_tblEntryComment_SnusPunchUserModelIdRepliedTo",
                table: "tblEntryComment");

            migrationBuilder.DropColumn(
                name: "SnusPunchUserModelIdRepliedTo",
                table: "tblEntryComment");
        }
    }
}
