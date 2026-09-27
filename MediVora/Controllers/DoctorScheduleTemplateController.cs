using MediVora.DTO;
using MediVora.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediVora.Controllers
{
    [Route("api/admin/doctors/{doctorId}/schedule-templates")]
    [ApiController]
    public class DoctorScheduleTemplateController : ControllerBase
    {
        public readonly IDoctorScheduleTemplateService _doctorScheduleTemplateService;
        public DoctorScheduleTemplateController(IDoctorScheduleTemplateService doctorScheduleTemplateService)
        {
            _doctorScheduleTemplateService = doctorScheduleTemplateService;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorScheduleTemplates(int doctorId)
        {
            var vTemplates = await _doctorScheduleTemplateService.GetAllDoctorScheduleTemplatesAsync(doctorId);
            return Ok(vTemplates);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorScheduleTemplate(int doctorId, int id)
        {
            var vTemplate = await _doctorScheduleTemplateService.GetDoctorScheduleTemplateByIdAsync(doctorId, id);
            return Ok(vTemplate);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateDoctorScheduleTemplate(int doctorId, [FromBody] DoctorScheduleTemplateAddEditDTO d)
        {
            var vResult = await _doctorScheduleTemplateService.CreateDoctorScheduleTemplateAsync(doctorId, d);
            return Ok(vResult);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctorScheduleTemplate(int doctorId, int id, [FromBody] DoctorScheduleTemplateAddEditDTO d)
        {
            var vResult = await _doctorScheduleTemplateService.UpdateDoctorScheduleTemplateAsync(doctorId, id, d);
            return Ok(vResult);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctorScheduleTemplate(int doctorId, int id)
        {
            var vResult = await _doctorScheduleTemplateService.DeleteDoctorScheduleTemplateAsync(doctorId, id);
            return Ok(vResult);
        }


        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatusDoctorScheduleTemplate(int doctorId, int id, [FromBody] bool isActive)
        {
            var vResult = await _doctorScheduleTemplateService.UpdateDoctorScheduleTemplateStatusAsync(doctorId, id, isActive);
            return Ok(vResult);
        }
    }
}
