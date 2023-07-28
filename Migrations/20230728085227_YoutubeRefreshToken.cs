using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaManageAPI.Migrations
{
    public partial class YoutubeRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "YoutubeRefreshToken",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YoutubeRefreshToken",
                table: "AspNetUsers");
        }
    }
}
