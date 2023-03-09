using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryControl.Migrations
{
    public partial class DeviceHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceHistories_Tickets_TicketId",
                table: "DeviceHistories");

            migrationBuilder.DropColumn(
                name: "IsRepaired",
                table: "DeviceHistories");

            migrationBuilder.RenameColumn(
                name: "TicketId",
                table: "DeviceHistories",
                newName: "DeviceId");

            migrationBuilder.RenameColumn(
                name: "RepairedDate",
                table: "DeviceHistories",
                newName: "CreatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceHistories_TicketId",
                table: "DeviceHistories",
                newName: "IX_DeviceHistories_DeviceId");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "DeviceHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "DeviceHistories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceHistories_UserId",
                table: "DeviceHistories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceHistories_AspNetUsers_UserId",
                table: "DeviceHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceHistories_Devices_DeviceId",
                table: "DeviceHistories",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceHistories_AspNetUsers_UserId",
                table: "DeviceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceHistories_Devices_DeviceId",
                table: "DeviceHistories");

            migrationBuilder.DropIndex(
                name: "IX_DeviceHistories_UserId",
                table: "DeviceHistories");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "DeviceHistories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DeviceHistories");

            migrationBuilder.RenameColumn(
                name: "DeviceId",
                table: "DeviceHistories",
                newName: "TicketId");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "DeviceHistories",
                newName: "RepairedDate");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceHistories_DeviceId",
                table: "DeviceHistories",
                newName: "IX_DeviceHistories_TicketId");

            migrationBuilder.AddColumn<bool>(
                name: "IsRepaired",
                table: "DeviceHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceHistories_Tickets_TicketId",
                table: "DeviceHistories",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
