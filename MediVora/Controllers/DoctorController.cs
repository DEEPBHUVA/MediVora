using MediVora.DTO;
using MediVora.Extensions.RateLimiting;
using MediVora.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MediVora.Controllers
{
    [ApiController]
    [Route("api")]
    public class DoctorController : ControllerBase
    {
        public readonly IDoctorService _doctorService;
        public readonly IFileService _fileService;
        public DoctorController(IDoctorService doctorService, IFileService fileService)
        {
            _doctorService = doctorService;
            _fileService = fileService;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("admin/doctors")]
        public async Task<IActionResult> GetDoctorsAsync()
        {
            var response = await _doctorService.GetAllDoctorsAsync();
            return Ok(response);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("admin/doctors/{id}")]
        public async Task<IActionResult> GetDoctorByIdAsync(int id)
        {
            var response = await _doctorService.GetDoctorByIdAsync(id);
            return Ok(response);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("admin/doctors")]
        public async Task<IActionResult> CreateDoctorAsync([FromForm] DoctorAddDTO doctorRequestDTO)
        {
            string? profileImagePath = null;

            if (doctorRequestDTO.ProfileImage != null)
            {
                profileImagePath = await _fileService.UploadFileAsync(doctorRequestDTO.ProfileImage, "doctors");
            }

            doctorRequestDTO.ProfileImagePath = profileImagePath;

            var response = await _doctorService.CreateDoctorAsync(doctorRequestDTO);
            return Ok(response);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("admin/doctors/{id}")]
        public async Task<IActionResult> UpdateDoctorAsync(int id, [FromForm] DoctorEditDTO doctorRequestDTO)
        {
            string? profileImagePath = null;

            if (doctorRequestDTO.ProfileImage != null)
            {
                profileImagePath = await _fileService.UploadFileAsync(doctorRequestDTO.ProfileImage, "doctors");
            }

            doctorRequestDTO.ProfileImagePath = profileImagePath;

            var response = await _doctorService.UpdateDoctorAsync(id, doctorRequestDTO);
            return Ok(response);
        }


        [Authorize(Roles = "Admin")]
        [HttpPatch("admin/doctors/{id}")]
        public async Task<IActionResult> DeleteDoctorAsync(int id)
        {
            var response = await _doctorService.DeleteDoctorAsync(id);
            return Ok(response);
        }


        [AllowAnonymous]
        [HttpGet("doctors/search")]
        [EnableRateLimiting(RateLimitPolicies.DoctorSearch)]
        public async Task<IActionResult> SearchDoctorsAsync([FromQuery] DoctorSearchFilterDTO filterDTO)
        {
            var response = await _doctorService.GetDoctorSearchByFilterAsync(filterDTO);

            return Ok(response);
        }

    }
}
