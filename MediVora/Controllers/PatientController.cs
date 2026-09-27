using MediVora.DTO;
using MediVora.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediVora.Controllers
{
    [ApiController]
    public class PatientController : ControllerBase
    {
        public readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("api/admin/patients")]
        public async Task<IActionResult> GetAllPatientsAsync()
        {
            var vResult = await _patientService.GetPatientsAsync();
            return Ok(vResult);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("api/admin/patients/{id}")]
        public async Task<IActionResult> GetPatientByIDAsync(int id)
        {
            var vResult = await _patientService.GetPatientByIdAsync(id);
            return Ok(vResult);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("api/admin/patients")]
        public async Task<IActionResult> CreatePatientAsync([FromBody] PatientAddDTO d)
        {
            var vResult = await _patientService.CreatePatientProfileAsync(d);
            return Ok(vResult);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("api/admin/patients/{id}")]
        public async Task<IActionResult> UpdatePatientAsync(int id, [FromBody] PatientEditDTO d)
        {
            var vResult = await _patientService.UpdatePatientProfileAsync(id, d);
            return Ok(vResult);
        }


        [Authorize(Roles = "Admin")]
        [HttpPatch("api/admin/patients/{id}/status")]
        public async Task<IActionResult> UpdatePatientStatusAsync(int id, [FromBody] bool IsActive)
        {
            var vResult = await _patientService.UpdatePatientStatusAsync(id, IsActive);
            return Ok(vResult);
        }


        [Authorize(Roles = "Patient")]
        [HttpGet("api/patients/me")]
        public async Task<IActionResult> GetLoggedInPatientDetails()
        {
            var vResult = await _patientService.GetCurrentPatientAsync();
            return Ok(vResult);
        }


        [Authorize(Roles = "Patient")]
        [HttpPut("api/patient/profile")]
        public async Task<IActionResult> UpdateMyProfileAsync([FromBody] PatientEditDTO d)
        {
            var vResult = await _patientService.UpdatePatientProfileAsync(null, d);
            return Ok(vResult);
        }

    }
}
