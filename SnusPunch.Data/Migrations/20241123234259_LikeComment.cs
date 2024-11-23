using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnusPunch.Data.Migrations
{
    /// <inheritdoc />
    public partial class LikeComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblEntryComment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryId = table.Column<int>(type: "int", nullable: false),
                    SnusPunchUserModelId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEntryComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblEntryComment_AspNetUsers_SnusPunchUserModelId",
                        column: x => x.SnusPunchUserModelId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tblEntryComment_tblEntry_EntryId",
                        column: x => x.EntryId,
                        principalTable: "tblEntry",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "tblEntryLike",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntryId = table.Column<int>(type: "int", nullable: false),
                    SnusPunchUserModelId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEntryLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblEntryLike_AspNetUsers_SnusPunchUserModelId",
                        column: x => x.SnusPunchUserModelId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tblEntryLike_tblEntry_EntryId",
                        column: x => x.EntryId,
                        principalTable: "tblEntry",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblEntryComment_EntryId",
                table: "tblEntryComment",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEntryComment_SnusPunchUserModelId",
                table: "tblEntryComment",
                column: "SnusPunchUserModelId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEntryLike_EntryId_SnusPunchUserModelId",
                table: "tblEntryLike",
                columns: new[] { "EntryId", "SnusPunchUserModelId" },
                unique: true,
                filter: "[SnusPunchUserModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tblEntryLike_SnusPunchUserModelId",
                table: "tblEntryLike",
                column: "SnusPunchUserModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblEntryComment");

            migrationBuilder.DropTable(
                name: "tblEntryLike");
        }
    }
}
