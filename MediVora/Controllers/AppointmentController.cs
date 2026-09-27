using MediVora.DTO;
using MediVora.Extensions.RateLimiting;
using MediVora.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MediVora.Controllers
{
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        public readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("api/admin/appointments")]
        [EnableRateLimiting(RateLimitPolicies.AppointmentBooking)]
        public async Task<IActionResult> CreateAppointmentByAdmin(AdminAppointmentAddDTO d)
        {
            var vResult = await _appointmentService.CreateAppointmentByAdminAsync(d);
            return Ok(vResult);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("api/admin/appointments/cancel")]
        public async Task<IActionResult> CancelAppointmentByAdmin(AppointmentCancelDTO d)
        {
            var vResult = await _appointmentService.CancelAppointmentByAdminAsync(d);
            return Ok(vResult);
        }


        [Authorize(Roles = "Admin,Doctor")]
        [HttpPut("api/appointments/status")]
        public async Task<IActionResult> UpdateAppointmentStatusAsync(AppointmentStatusUpdateDTO d)
        {
            var vResult = await _appointmentService.UpdateAppointmentStatusAsync(d);
            return Ok(vResult);
        }

        //============================ Doctor ============================

        [Authorize(Roles = "Doctor")]
        [HttpGet("api/doctors/me/appointments")]
        public async Task<IActionResult> GetLoggedInDoctorAppointments([FromQuery] DoctorAppointmentSearchFilterDTO d)
        {
            var vResult = await _appointmentService.GetLoggedInDoctorAppointmentsAsync(d);
            return Ok(vResult);
        }


        [Authorize(Roles = "Doctor")]
        [HttpGet("api/doctors/me/appointments/{AppointmentID}")]
        public async Task<IActionResult> GetLoggedInDoctorAppointmentById(int AppointmentID)
        {
            var vResult = await _appointmentService.GetLoggedInDoctorAppointmentByIdAsync(AppointmentID);
            return Ok(vResult);
        }


        //============================ Patient ============================

        [Authorize(Roles = "Patient")]
        [HttpPost("api/patients/me/appointments")]
        [EnableRateLimiting(RateLimitPolicies.AppointmentBooking)]
        public async Task<IActionResult> CreateAppointmentByPatient(PatientAppointmentAddDTO d)
        {
            var vResult = await _appointmentService.CreateAppointmentByPatientAsync(d);
            return Ok(vResult);
        }


        [Authorize(Roles = "Patient")]
        [HttpGet("api/patients/me/appointments/{AppointmentID}")]
        public async Task<IActionResult> GetLoggedInPatientAppointmentById(int AppointmentID)
        {
            var vResult = await _appointmentService.GetLoggedInPatientAppointmentByIdAsync(AppointmentID);
            return Ok(vResult);
        }


        [Authorize(Roles = "Patient")]
        [HttpGet("api/patients/me/appointments")]
        public async Task<IActionResult> GetLoggedInPatientAppointments([FromQuery] AppointmentSearchFilterDTO d)
        {
            var vResult = await _appointmentService.GetLoggedInPatientAppointmentsAsync(d);
            return Ok(vResult);
        }


        [Authorize(Roles = "Patient")]
        [HttpPut("api/patient/me/appointments/cancel")]
        public async Task<IActionResult> CancelAppointmentByPatient(AppointmentCancelDTO d)
        {
            var vResult = await _appointmentService.CancelAppointmentByPatientAsync(d);
            return Ok(vResult);
        }
    }
}
