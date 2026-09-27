using MediVora.Common;
using MediVora.DTO;

namespace MediVora.Interfaces
{
    public interface IUserService
    {
        public Task<ApiResponse<UserResponseDTO>> GetUserByIdAsync(int userId);
        public Task<ApiResponse<UserResponseDTO>> GetUserByEmailAsync(string email);
        public Task<ApiResponse<List<UserResponseDTO>>> GetAllUsersAsync();
        public Task<ApiResponse<List<UserResponseDTO>>> GetAllUsersByRoleAsync(int roleId);
    }
}
