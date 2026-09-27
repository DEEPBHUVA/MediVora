using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediVora.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipbetweenuserandrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                schema: "dbo",
                table: "Users",
                column: "RoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleID",
                schema: "dbo",
                table: "Users",
                column: "RoleID",
                principalSchema: "dbo",
                principalTable: "Roles",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleID",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleID",
                schema: "dbo",
                table: "Users");
        }
    }
}
