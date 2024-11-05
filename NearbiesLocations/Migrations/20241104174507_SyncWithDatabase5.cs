using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NearbiesLocations.Migrations
{
    public partial class SyncWithDatabase5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_SearchRequests_Users_UserID",
                table: "SearchRequests",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {           
            migrationBuilder.AddForeignKey(
                name: "FK_SearchRequests_Users_UserId",
                table: "SearchRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
