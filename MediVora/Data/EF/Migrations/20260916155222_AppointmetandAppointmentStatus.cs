using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediVora.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class AppointmetandAppointmentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppointmentStatus",
                schema: "dbo",
                columns: table => new
                {
                    AppointmentStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentStatus", x => x.AppointmentStatusID);
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                schema: "dbo",
                columns: table => new
                {
                    AppointmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentNo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DoctorScheduleID = table.Column<int>(type: "int", nullable: false),
                    DoctorID = table.Column<int>(type: "int", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    AppointmentStatusID = table.Column<int>(type: "int", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time(0)", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time(0)", nullable: false),
                    AppointmentType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PatientRemarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DoctorRemarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BookedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ConfirmedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CheckedInDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    StartedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CancelledDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.AppointmentID);
                    table.ForeignKey(
                        name: "FK_Appointment_AppointmentStatus_AppointmentStatusID",
                        column: x => x.AppointmentStatusID,
                        principalSchema: "dbo",
                        principalTable: "AppointmentStatus",
                        principalColumn: "AppointmentStatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointment_DoctorSchedules_DoctorScheduleID",
                        column: x => x.DoctorScheduleID,
                        principalSchema: "dbo",
                        principalTable: "DoctorSchedules",
                        principalColumn: "DoctorScheduleID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointment_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalSchema: "dbo",
                        principalTable: "Doctors",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointment_Patients_PatientID",
                        column: x => x.PatientID,
                        principalSchema: "dbo",
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_AppointmentNo",
                schema: "dbo",
                table: "Appointment",
                column: "AppointmentNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_AppointmentStatusID",
                schema: "dbo",
                table: "Appointment",
                column: "AppointmentStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_DoctorID",
                schema: "dbo",
                table: "Appointment",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_DoctorScheduleID",
                schema: "dbo",
                table: "Appointment",
                column: "DoctorScheduleID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_PatientID",
                schema: "dbo",
                table: "Appointment",
                column: "PatientID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointment",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppointmentStatus",
                schema: "dbo");
        }
    }
}
