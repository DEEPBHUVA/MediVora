using MediVora.Interfaces;
using System.Security.Claims;

namespace MediVora.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        public int UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!int.TryParse(userIdClaim, out var userId))
                {
                    throw new UnauthorizedAccessException(
                        "User ID not found in authentication token.");
                }

                return userId;
            }
        }

        public string? Email => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
        public bool? IsAdmin => _httpContextAccessor.HttpContext?.User.IsInRole("Admin") ?? false;
        public bool? IsDoctor => _httpContextAccessor.HttpContext?.User.IsInRole("Doctor") ?? false;
        public bool? IsPatient => _httpContextAccessor.HttpContext?.User.IsInRole("Patient") ?? false;
    }
}
