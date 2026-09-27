using MediVora.DTO;
using MediVora.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediVora.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/appointmentstatus")]
    public class AppointmentStatusController : ControllerBase
    {
        private readonly IAppointmentStatusService _appointmentStatusService;

        public AppointmentStatusController(IAppointmentStatusService appointmentStatusService)
        {
            _appointmentStatusService = appointmentStatusService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointmentStatus(AppointmentStatusAddEditDTO d)
        {
            var response = await _appointmentStatusService.CreateAppointmentStatusAsync(d);

            return Ok(response);
        }

        [HttpPut("{AppointmentStatusID}")]
        public async Task<IActionResult> UpdateAppointmentStatus(
            int AppointmentStatusID,
            AppointmentStatusAddEditDTO d)
        {
            var response = await _appointmentStatusService.UpdateAppointmentStatusAsync(
                AppointmentStatusID,
                d);

            return Ok(response);
        }

        [HttpDelete("{AppointmentStatusID}")]
        public async Task<IActionResult> DeleteAppointmentStatus(int AppointmentStatusID)
        {
            var response = await _appointmentStatusService.DeleteAppointmentStatusAsync(
                AppointmentStatusID);

            return Ok(response);
        }

        [HttpGet("{AppointmentStatusID}")]
        public async Task<IActionResult> GetAppointmentStatusById(int AppointmentStatusID)
        {
            var response = await _appointmentStatusService.GetAppointmentStatusByIdAsync(
                AppointmentStatusID);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointmentStatus()
        {
            var response = await _appointmentStatusService.GetAllAppointmentStatusAsync();

            return Ok(response);
        }

        [HttpGet("dropdown")]
        public async Task<IActionResult> GetAppointmentStatusDropdown()
        {
            var response = await _appointmentStatusService.GetAppointmentStatusDropdownAsync();

            return Ok(response);
        }
    }
}
    