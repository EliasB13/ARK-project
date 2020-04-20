using Microsoft.EntityFrameworkCore.Migrations;

namespace ARK_Backend.Infrastructure.Data.Migrations
{
    public partial class cascade_added_pc_and_employee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeesRoles_BusinessUsers_BusinessUserId",
                table: "EmployeesRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeesRoles_BusinessUsers_BusinessUserId",
                table: "EmployeesRoles",
                column: "BusinessUserId",
                principalTable: "BusinessUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeesRoles_BusinessUsers_BusinessUserId",
                table: "EmployeesRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeesRoles_BusinessUsers_BusinessUserId",
                table: "EmployeesRoles",
                column: "BusinessUserId",
                principalTable: "BusinessUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
