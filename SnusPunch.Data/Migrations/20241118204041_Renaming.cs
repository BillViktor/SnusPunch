using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnusPunch.Data.Migrations
{
    /// <inheritdoc />
    public partial class Renaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "tblSnus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 18, 21, 40, 40, 804, DateTimeKind.Local).AddTicks(5219),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 18, 21, 39, 47, 427, DateTimeKind.Local).AddTicks(6196));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "tblEntry",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 18, 21, 40, 40, 803, DateTimeKind.Local).AddTicks(3196),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 18, 21, 39, 47, 426, DateTimeKind.Local).AddTicks(4470));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "tblSnus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 18, 21, 39, 47, 427, DateTimeKind.Local).AddTicks(6196),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 18, 21, 40, 40, 804, DateTimeKind.Local).AddTicks(5219));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "tblEntry",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 18, 21, 39, 47, 426, DateTimeKind.Local).AddTicks(4470),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 18, 21, 40, 40, 803, DateTimeKind.Local).AddTicks(3196));
        }
    }
}
