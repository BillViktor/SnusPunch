using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnusPunch.Data.Migrations
{
    /// <inheritdoc />
    public partial class Entry2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblEntry_AspNetUsers_SnusPunchUserModelId",
                table: "tblEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_tblEntry_tblSnus_SnusId",
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
                oldDefaultValue: new DateTime(2024, 11, 18, 21, 3, 27, 47, DateTimeKind.Local).AddTicks(1771));

            migrationBuilder.AlterColumn<string>(
                name: "SnusPunchUserId",
                table: "tblEntry",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "tblEntry",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 18, 21, 8, 2, 231, DateTimeKind.Local).AddTicks(7739),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 18, 21, 3, 27, 46, DateTimeKind.Local).AddTicks(9967));

            migrationBuilder.CreateIndex(
                name: "IX_tblEntry_SnusPunchUserId",
                table: "tblEntry",
                column: "SnusPunchUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblEntry_AspNetUsers_SnusPunchUserId",
                table: "tblEntry",
                column: "SnusPunchUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblEntry_tblSnus_SnusId",
                table: "tblEntry",
                column: "SnusId",
                principalTable: "tblSnus",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblEntry_AspNetUsers_SnusPunchUserId",
                table: "tblEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_tblEntry_tblSnus_SnusId",
                table: "tblEntry");

            migrationBuilder.DropIndex(
                name: "IX_tblEntry_SnusPunchUserId",
                table: "tblEntry");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "tblSnus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 18, 21, 3, 27, 47, DateTimeKind.Local).AddTicks(1771),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 18, 21, 8, 2, 232, DateTimeKind.Local).AddTicks(2519));

            migrationBuilder.AlterColumn<int>(
                name: "SnusPunchUserId",
                table: "tblEntry",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "tblEntry",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 18, 21, 3, 27, 46, DateTimeKind.Local).AddTicks(9967),
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

            migrationBuilder.AddForeignKey(
                name: "FK_tblEntry_tblSnus_SnusId",
                table: "tblEntry",
                column: "SnusId",
                principalTable: "tblSnus",
                principalColumn: "Id");
        }
    }
}
