using MediVora.DTO;
using MediVora.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediVora.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        public readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _roleService.GetAllRolesAsync();
            return Ok(response);
        }


        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRoleById(int roleId)
        {
            var response = await _roleService.GetRoleByIdAsync(roleId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleAddEditDTO roleRequest)
        {
            var response = await _roleService.CreateRoleAsync(roleRequest);
            return Ok(response);
        }

        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateRole(int roleId, [FromBody] RoleAddEditDTO roleRequest)
        {
            var response = await _roleService.UpdateRoleAsync(roleId, roleRequest);
            return Ok(response);
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            var response = await _roleService.DeleteRoleAsync(roleId);
            return Ok(response);
        }
    }
}
