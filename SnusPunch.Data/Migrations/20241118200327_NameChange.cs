using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnusPunch.Data.Migrations
{
    /// <inheritdoc />
    public partial class NameChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Snus_FavoriteSnusId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_tblEntry_Snus_SnusId",
                table: "tblEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Snus",
                table: "tblSnus");

            //migrationBuilder.RenameTable(
            //    name: "Snus",
            //    newName: "tblSnus");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "tblEntry",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 18, 21, 3, 27, 46, DateTimeKind.Local).AddTicks(9967),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 11, 18, 20, 56, 51, 489, DateTimeKind.Local).AddTicks(117));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "tblSnus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 18, 21, 3, 27, 47, DateTimeKind.Local).AddTicks(1771),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 11, 18, 20, 56, 51, 489, DateTimeKind.Local).AddTicks(1958));

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblSnus",
                table: "tblSnus",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_tblSnus_FavoriteSnusId",
                table: "AspNetUsers",
                column: "FavoriteSnusId",
                principalTable: "tblSnus",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_tblEntry_tblSnus_SnusId",
                table: "tblEntry",
                column: "SnusId",
                principalTable: "tblSnus",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_tblSnus_FavoriteSnusId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_tblEntry_tblSnus_SnusId",
                table: "tblEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblSnus",
                table: "tblSnus");

            migrationBuilder.RenameTable(
                name: "tblSnus",
                newName: "Snus");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "tblEntry",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 11, 18, 20, 56, 51, 489, DateTimeKind.Local).AddTicks(117),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 18, 21, 3, 27, 46, DateTimeKind.Local).AddTicks(9967));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Snus",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 11, 18, 20, 56, 51, 489, DateTimeKind.Local).AddTicks(1958),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 18, 21, 3, 27, 47, DateTimeKind.Local).AddTicks(1771));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Snus",
                table: "Snus",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Snus_FavoriteSnusId",
                table: "AspNetUsers",
                column: "FavoriteSnusId",
                principalTable: "Snus",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_tblEntry_Snus_SnusId",
                table: "tblEntry",
                column: "SnusId",
                principalTable: "Snus",
                principalColumn: "Id");
        }
    }
}
