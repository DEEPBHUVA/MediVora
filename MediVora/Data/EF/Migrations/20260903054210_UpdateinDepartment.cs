using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediVora.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class UpdateinDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DepartmentCode",
                schema: "dbo",
                table: "Departments",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DepartmentCode",
                schema: "dbo",
                table: "Departments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");
        }
    }
}
