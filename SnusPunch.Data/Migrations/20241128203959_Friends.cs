using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnusPunch.Data.Migrations
{
    /// <inheritdoc />
    public partial class Friends : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblSnusPunchUserFriend",
                columns: table => new
                {
                    SnusPunchUserModelOneId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SnusPunchUserModelTwoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSnusPunchUserFriend", x => new { x.SnusPunchUserModelOneId, x.SnusPunchUserModelTwoId });
                    table.ForeignKey(
                        name: "FK_tblSnusPunchUserFriend_AspNetUsers_SnusPunchUserModelOneId",
                        column: x => x.SnusPunchUserModelOneId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tblSnusPunchUserFriend_AspNetUsers_SnusPunchUserModelTwoId",
                        column: x => x.SnusPunchUserModelTwoId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblSnusPunchUserFriend_SnusPunchUserModelTwoId",
                table: "tblSnusPunchUserFriend",
                column: "SnusPunchUserModelTwoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblSnusPunchUserFriend");
        }
    }
}
