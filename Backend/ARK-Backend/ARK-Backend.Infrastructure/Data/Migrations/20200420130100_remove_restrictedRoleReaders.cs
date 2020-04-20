using Microsoft.EntityFrameworkCore.Migrations;

namespace ARK_Backend.Infrastructure.Data.Migrations
{
    public partial class remove_restrictedRoleReaders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllowedRoleReaders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllowedRoleReaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeesRoleId = table.Column<int>(type: "int", nullable: true),
                    ReaderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowedRoleReaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllowedRoleReaders_EmployeesRoles_EmployeesRoleId",
                        column: x => x.EmployeesRoleId,
                        principalTable: "EmployeesRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AllowedRoleReaders_Readers_ReaderId",
                        column: x => x.ReaderId,
                        principalTable: "Readers",
                        principalColumn: "ReaderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllowedRoleReaders_EmployeesRoleId",
                table: "AllowedRoleReaders",
                column: "EmployeesRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AllowedRoleReaders_ReaderId",
                table: "AllowedRoleReaders",
                column: "ReaderId");
        }
    }
}
