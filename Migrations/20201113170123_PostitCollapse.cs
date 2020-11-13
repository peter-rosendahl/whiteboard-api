using Microsoft.EntityFrameworkCore.Migrations;

namespace Whiteboard.API.Migrations
{
    public partial class PostitCollapse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCollapsed",
                table: "Postit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCollapsed",
                table: "Postit");
        }
    }
}
