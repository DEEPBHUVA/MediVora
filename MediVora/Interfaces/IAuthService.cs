using MediVora.Common;
using MediVora.DTO;

namespace MediVora.Interfaces
{
    public interface IAuthService
    {
        public Task<RegisterResponse> RegisterAsync(RegisterDTO registerDTO);
        public Task<LoginResponse> LoginAsync(LoginDTO loginDTO);
        public Task<ApiResponse<UserResponseDTO>> GetLoggedInUserAsync(string token);
    }
}
