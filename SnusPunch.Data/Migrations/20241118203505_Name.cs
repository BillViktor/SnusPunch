using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnusPunch.Data.Migrations
{
    /// <inheritdoc />
    public partial class Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "tblSnus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 18, 21, 35, 5, 227, DateTimeKind.Local).AddTicks(6494),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 18, 21, 8, 2, 232, DateTimeKind.Local).AddTicks(2519));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "tblEntry",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 18, 21, 35, 5, 226, DateTimeKind.Local).AddTicks(3721),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 18, 21, 8, 2, 231, DateTimeKind.Local).AddTicks(7739));

            migrationBuilder.AddColumn<string>(
                name: "SnusPunchUserModelId",
                table: "tblEntry",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblEntry_SnusPunchUserModelId",
                table: "tblEntry",
                column: "SnusPunchUserModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblEntry_AspNetUsers_SnusPunchUserModelId",
                table: "tblEntry",
                column: "SnusPunchUserModelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblEntry_AspNetUsers_SnusPunchUserModelId",
                table: "tblEntry");

            migrationBuilder.DropIndex(
                name: "IX_tblEntry_SnusPunchUserModelId",
                table: "tblEntry");

            migrationBuilder.DropColumn(
                name: "SnusPunchUserModelId",
                table: "tblEntry");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "tblSnus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 18, 21, 8, 2, 232, DateTimeKind.Local).AddTicks(2519),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 18, 21, 35, 5, 227, DateTimeKind.Local).AddTicks(6494));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "tblEntry",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 18, 21, 8, 2, 231, DateTimeKind.Local).AddTicks(7739),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 18, 21, 35, 5, 226, DateTimeKind.Local).AddTicks(3721));
        }
    }
}
