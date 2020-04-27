using Microsoft.EntityFrameworkCore.Migrations;

namespace ARK_Backend.Infrastructure.Data.Migrations
{
    public partial class added_photo_field_personcard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "BusinessUsers");

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "PersonCards",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "PersonCards");

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "BusinessUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
