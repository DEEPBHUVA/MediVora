using MediVora.Common;
using MediVora.DTO;

namespace MediVora.Interfaces
{
    public interface IRoleService
    {
        public Task<ApiResponse<List<RoleResponseDTO>>> GetAllRolesAsync();
        public Task<ApiResponse<RoleResponseDTO>> GetRoleByIdAsync(int roleId);
        public Task<ApiResponse<RoleResponseDTO>> CreateRoleAsync(RoleAddEditDTO roleRequest);
        public Task<ApiResponse<RoleResponseDTO>> UpdateRoleAsync(int roleId, RoleAddEditDTO roleRequest);
        public Task<ApiResponse<bool>> DeleteRoleAsync(int roleId);
    }
}
