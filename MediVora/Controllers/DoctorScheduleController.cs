using MediVora.DTO;
using MediVora.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediVora.Controllers
{
    [ApiController]
    public class DoctorScheduleController : ControllerBase
    {
        public readonly IDoctorScheduleService _doctorScheduleService;
        public DoctorScheduleController(IDoctorScheduleService doctorScheduleService)
        {
            _doctorScheduleService = doctorScheduleService;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("api/admin/doctors/{doctorId}/schedules")]
        public async Task<IActionResult> GetDoctorSchedules(int doctorId)
        {
            var schedules = await _doctorScheduleService.GetDoctorSchedulesAsync(doctorId);
            return Ok(schedules);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("api/admin/doctors/{doctorId}/schedules/{id}")]
        public async Task<IActionResult> GetDoctorSchedule(int doctorId, int id)
        {
            var schedule = await _doctorScheduleService.GetDoctorScheduleByIdAsync(doctorId, id);
            return Ok(schedule);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("api/admin/doctors/{doctorId}/schedules")]
        public async Task<IActionResult> CreateDoctorSchedule(int doctorId, [FromBody] DoctorScheduleAddEditDTO schedule)
        {
            var createdSchedule = await _doctorScheduleService.CreateDoctorScheduleAsync(doctorId, schedule);
            return Ok(createdSchedule);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("api/admin/doctors/{doctorId}/schedules/{id}")]
        public async Task<IActionResult> UpdateDoctorSchedule(int doctorId, int id, [FromBody] DoctorScheduleAddEditDTO schedule)
        {
            var updatedSchedule = await _doctorScheduleService.UpdateDoctorScheduleAsync(doctorId, id, schedule);
            return Ok(updatedSchedule);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("api/admin/doctors/{doctorId}/schedules/{id}")]
        public async Task<IActionResult> DeleteDoctorSchedule(int doctorId, int id)
        {
            await _doctorScheduleService.DeleteDoctorScheduleAsync(doctorId, id);
            return NoContent();
        }


        [Authorize(Roles = "Admin")]
        [HttpPatch("api/admin/doctors/{doctorId}/schedules/{id}/status")]
        public async Task<IActionResult> ChangeStatusDoctorSchedule(int doctorId, int id, [FromBody] bool IsAvailable)
        {
            var updatedSchedule = await _doctorScheduleService.UpdateDoctorScheduleStatusAsync(doctorId, id, IsAvailable);
            return Ok(updatedSchedule);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("api/admin/doctors/{doctorId}/schedules/generate")]
        public async Task<IActionResult> GenerateDoctorSchedule(int doctorId, GenerateDoctorScheduleDTO d)
        {
            var generatedSchedule = await _doctorScheduleService.GenerateDoctorScheduleAsync(doctorId, d);
            return Ok(generatedSchedule);
        }


        [Authorize(Roles = "Doctor")]
        [HttpGet("api/doctors/me/schedules")]
        public async Task<IActionResult> GetLoggedDoctorSchedules()
        {
            var schedules = await _doctorScheduleService.GetDoctorSchedulesAsync();
            return Ok(schedules);
        }


        [Authorize(Roles = "Doctor")]
        [HttpGet("api/doctors/me/schedules/by-date-range")]
        public async Task<IActionResult> GetLoggedDoctorSchedule([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            var schedule = await _doctorScheduleService.GetDoctorSchedulesByDateRangeAsync(fromDate, toDate);

            return Ok(schedule);
        }
    }
}
