using MediVora.DTO;
using MediVora.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediVora.Controllers
{
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("api/departments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var response = await _departmentService.GetAllDepartmentsAsync();
            return Ok(response);
        }

        [HttpGet("api/departments/{departmentId}")]
        public async Task<IActionResult> GetDepartmentById(int departmentId)
        {
            var response = await _departmentService.GetDepartmentByIdAsync(departmentId);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("api/admin/departments")]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentAddEditDTO departmentRequestDTO)
        {
            var response = await _departmentService.CreateDepartmentAsync(departmentRequestDTO);
            return Ok(response);
        }

        [Authorize]
        [HttpPut("api/admin/departments/{departmentId}")]
        public async Task<IActionResult> UpdateDepartment(int departmentId, [FromBody] DepartmentAddEditDTO departmentRequestDTO)
        {
            var response = await _departmentService.UpdateDepartmentAsync(departmentId, departmentRequestDTO);
            return Ok(response);
        }

        [Authorize]
        [HttpPatch("api/admin/departments/{departmentId}")]
        public async Task<IActionResult> DeleteDepartment(int departmentId)
        {
            var response = await _departmentService.DeleteDepartmentAsync(departmentId);
            return Ok(response);
        }
    }
}
