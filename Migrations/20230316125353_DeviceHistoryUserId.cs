using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryControl.Migrations
{
    public partial class DeviceHistoryUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceHistories_AspNetUsers_UserId",
                table: "DeviceHistories");

            migrationBuilder.DropIndex(
                name: "IX_DeviceHistories_UserId",
                table: "DeviceHistories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DeviceHistories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "DeviceHistories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceHistories_UserId",
                table: "DeviceHistories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceHistories_AspNetUsers_UserId",
                table: "DeviceHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
