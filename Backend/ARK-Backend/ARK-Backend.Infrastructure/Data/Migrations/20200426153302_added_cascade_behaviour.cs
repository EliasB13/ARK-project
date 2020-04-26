using Microsoft.EntityFrameworkCore.Migrations;

namespace ARK_Backend.Infrastructure.Data.Migrations
{
    public partial class added_cascade_behaviour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Observations_Readers_ReaderId",
                table: "Observations");

            migrationBuilder.DropForeignKey(
                name: "FK_Readers_BusinessUsers_BusinessUserId",
                table: "Readers");

            migrationBuilder.DropForeignKey(
                name: "FK_RestrictedRoleReaders_Readers_ReaderId",
                table: "RestrictedRoleReaders");

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_Readers_ReaderId",
                table: "Observations",
                column: "ReaderId",
                principalTable: "Readers",
                principalColumn: "ReaderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_BusinessUsers_BusinessUserId",
                table: "Readers",
                column: "BusinessUserId",
                principalTable: "BusinessUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RestrictedRoleReaders_Readers_ReaderId",
                table: "RestrictedRoleReaders",
                column: "ReaderId",
                principalTable: "Readers",
                principalColumn: "ReaderId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Observations_Readers_ReaderId",
                table: "Observations");

            migrationBuilder.DropForeignKey(
                name: "FK_Readers_BusinessUsers_BusinessUserId",
                table: "Readers");

            migrationBuilder.DropForeignKey(
                name: "FK_RestrictedRoleReaders_Readers_ReaderId",
                table: "RestrictedRoleReaders");

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_Readers_ReaderId",
                table: "Observations",
                column: "ReaderId",
                principalTable: "Readers",
                principalColumn: "ReaderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_BusinessUsers_BusinessUserId",
                table: "Readers",
                column: "BusinessUserId",
                principalTable: "BusinessUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RestrictedRoleReaders_Readers_ReaderId",
                table: "RestrictedRoleReaders",
                column: "ReaderId",
                principalTable: "Readers",
                principalColumn: "ReaderId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
