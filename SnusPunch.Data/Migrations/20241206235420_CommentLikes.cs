using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnusPunch.Data.Migrations
{
    /// <inheritdoc />
    public partial class CommentLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblEntryCommentLike",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntryCommentId = table.Column<int>(type: "int", nullable: false),
                    SnusPunchUserModelId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEntryCommentLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblEntryCommentLike_AspNetUsers_SnusPunchUserModelId",
                        column: x => x.SnusPunchUserModelId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tblEntryCommentLike_tblEntryComment_EntryCommentId",
                        column: x => x.EntryCommentId,
                        principalTable: "tblEntryComment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblEntryCommentLike_EntryCommentId_SnusPunchUserModelId",
                table: "tblEntryCommentLike",
                columns: new[] { "EntryCommentId", "SnusPunchUserModelId" },
                unique: true,
                filter: "[SnusPunchUserModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tblEntryCommentLike_SnusPunchUserModelId",
                table: "tblEntryCommentLike",
                column: "SnusPunchUserModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblEntryCommentLike");
        }
    }
}
