using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnusPunch.Data.Migrations
{
    /// <inheritdoc />
    public partial class NotificationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblSnusPunchUserNotifications_tblEntry_EntryModelId",
                table: "tblSnusPunchUserNotifications");

            migrationBuilder.DropIndex(
                name: "IX_tblSnusPunchUserNotifications_EntryModelId",
                table: "tblSnusPunchUserNotifications");

            migrationBuilder.RenameColumn(
                name: "EntryModelId",
                table: "tblSnusPunchUserNotifications",
                newName: "EntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "tblSnusPunchUserNotifications",
                newName: "EntryModelId");

            migrationBuilder.CreateIndex(
                name: "IX_tblSnusPunchUserNotifications_EntryModelId",
                table: "tblSnusPunchUserNotifications",
                column: "EntryModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblSnusPunchUserNotifications_tblEntry_EntryModelId",
                table: "tblSnusPunchUserNotifications",
                column: "EntryModelId",
                principalTable: "tblEntry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
