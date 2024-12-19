using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnusPunch.Data.Migrations
{
    /// <inheritdoc />
    public partial class Notifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.CreateTable(
                name: "tblSnusPunchUserNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SnusPunchUserModelIdOne = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SnusPunchUserModelIdTwo = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NotificationActionEnum = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    NotificationViewed = table.Column<bool>(type: "bit", nullable: false),
                    EntryModelId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSnusPunchUserNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblSnusPunchUserNotifications_AspNetUsers_SnusPunchUserModelIdOne",
                        column: x => x.SnusPunchUserModelIdOne,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tblSnusPunchUserNotifications_AspNetUsers_SnusPunchUserModelIdTwo",
                        column: x => x.SnusPunchUserModelIdTwo,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tblSnusPunchUserNotifications_tblEntry_EntryModelId",
                        column: x => x.EntryModelId,
                        principalTable: "tblEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblSnusPunchUserNotifications_EntryModelId",
                table: "tblSnusPunchUserNotifications",
                column: "EntryModelId");

            migrationBuilder.CreateIndex(
                name: "IX_tblSnusPunchUserNotifications_SnusPunchUserModelIdOne",
                table: "tblSnusPunchUserNotifications",
                column: "SnusPunchUserModelIdOne");

            migrationBuilder.CreateIndex(
                name: "IX_tblSnusPunchUserNotifications_SnusPunchUserModelIdTwo",
                table: "tblSnusPunchUserNotifications",
                column: "SnusPunchUserModelIdTwo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblSnusPunchUserNotifications");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUserRoles",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");
        }
    }
}
