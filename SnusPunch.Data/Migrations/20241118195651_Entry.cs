using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnusPunch.Data.Migrations
{
    /// <inheritdoc />
    public partial class Entry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Snus",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 11, 18, 20, 56, 51, 489, DateTimeKind.Local).AddTicks(1958),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 11, 11, 17, 7, 58, 92, DateTimeKind.Local).AddTicks(2286));

            migrationBuilder.CreateTable(
                name: "tblEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SnusPunchUserId = table.Column<int>(type: "int", nullable: false),
                    SnusPunchUserModelId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SnusId = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValue: new DateTime(2024, 11, 18, 20, 56, 51, 489, DateTimeKind.Local).AddTicks(117)),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblEntry_AspNetUsers_SnusPunchUserModelId",
                        column: x => x.SnusPunchUserModelId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tblEntry_Snus_SnusId",
                        column: x => x.SnusId,
                        principalTable: "Snus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblEntry_SnusId",
                table: "tblEntry",
                column: "SnusId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEntry_SnusPunchUserModelId",
                table: "tblEntry",
                column: "SnusPunchUserModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblEntry");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Snus",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 11, 11, 17, 7, 58, 92, DateTimeKind.Local).AddTicks(2286),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 11, 18, 20, 56, 51, 489, DateTimeKind.Local).AddTicks(1958));
        }
    }
}
