using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediVora.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class DoctorScheduleTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DoctorScheduleTemplates",
                schema: "dbo",
                columns: table => new
                {
                    DoctorScheduleTemplateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorID = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    DayOfWeekName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    SlotDurationInMinutes = table.Column<int>(type: "int", nullable: false),
                    MaxAppointmentsPerSlot = table.Column<int>(type: "int", nullable: false),
                    EffectiveFromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorScheduleTemplates", x => x.DoctorScheduleTemplateID);
                    table.ForeignKey(
                        name: "FK_DoctorScheduleTemplates_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalSchema: "dbo",
                        principalTable: "Doctors",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_UserID",
                schema: "dbo",
                table: "Doctors",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorScheduleTemplates_DoctorID",
                schema: "dbo",
                table: "DoctorScheduleTemplates",
                column: "DoctorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Users_UserID",
                schema: "dbo",
                table: "Doctors",
                column: "UserID",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Users_UserID",
                schema: "dbo",
                table: "Doctors");

            migrationBuilder.DropTable(
                name: "DoctorScheduleTemplates",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_UserID",
                schema: "dbo",
                table: "Doctors");
        }
    }
}
