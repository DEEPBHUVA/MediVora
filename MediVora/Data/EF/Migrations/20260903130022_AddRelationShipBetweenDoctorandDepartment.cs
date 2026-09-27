using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediVora.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationShipBetweenDoctorandDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Doctors_DepartmentID",
                schema: "dbo",
                table: "Doctors",
                column: "DepartmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Departments_DepartmentID",
                schema: "dbo",
                table: "Doctors",
                column: "DepartmentID",
                principalSchema: "dbo",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Departments_DepartmentID",
                schema: "dbo",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_DepartmentID",
                schema: "dbo",
                table: "Doctors");
        }
    }
}
