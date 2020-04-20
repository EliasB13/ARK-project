using Microsoft.EntityFrameworkCore.Migrations;

namespace ARK_Backend.Infrastructure.Data.Migrations
{
    public partial class add_anonym_cards1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Persons_PersonCardId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_Persons_PersonId",
                table: "Observations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Persons",
                table: "Persons");

            migrationBuilder.RenameTable(
                name: "Persons",
                newName: "PersonCards");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmployee",
                table: "PersonCards",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonCards",
                table: "PersonCards",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_PersonCards_PersonCardId",
                table: "Employees",
                column: "PersonCardId",
                principalTable: "PersonCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_PersonCards_PersonId",
                table: "Observations",
                column: "PersonId",
                principalTable: "PersonCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_PersonCards_PersonCardId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_PersonCards_PersonId",
                table: "Observations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonCards",
                table: "PersonCards");

            migrationBuilder.DropColumn(
                name: "IsEmployee",
                table: "PersonCards");

            migrationBuilder.RenameTable(
                name: "PersonCards",
                newName: "Persons");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Persons",
                table: "Persons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Persons_PersonCardId",
                table: "Employees",
                column: "PersonCardId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_Persons_PersonId",
                table: "Observations",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
