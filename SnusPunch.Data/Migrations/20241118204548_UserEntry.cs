using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnusPunch.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblEntry_AspNetUsers_SnusPunchUserModelId",
                table: "tblEntry");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "tblSnus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 18, 21, 45, 47, 911, DateTimeKind.Local).AddTicks(9022),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 18, 21, 40, 40, 804, DateTimeKind.Local).AddTicks(5219));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "tblEntry",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 18, 21, 45, 47, 911, DateTimeKind.Local).AddTicks(5405),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 18, 21, 40, 40, 803, DateTimeKind.Local).AddTicks(3196));

            migrationBuilder.AddForeignKey(
                name: "FK_tblEntry_AspNetUsers_SnusPunchUserModelId",
                table: "tblEntry",
                column: "SnusPunchUserModelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblEntry_AspNetUsers_SnusPunchUserModelId",
                table: "tblEntry");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "tblSnus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 18, 21, 40, 40, 804, DateTimeKind.Local).AddTicks(5219),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 18, 21, 45, 47, 911, DateTimeKind.Local).AddTicks(9022));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "tblEntry",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 18, 21, 40, 40, 803, DateTimeKind.Local).AddTicks(3196),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 18, 21, 45, 47, 911, DateTimeKind.Local).AddTicks(5405));

            migrationBuilder.AddColumn<string>(
                name: "SnusPunchUserId",
                table: "tblEntry",
                type: "nvarchar(450)",
                nullable: true);

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
                name: "FK_tblEntry_AspNetUsers_SnusPunchUserModelId",
                table: "tblEntry",
                column: "SnusPunchUserModelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
