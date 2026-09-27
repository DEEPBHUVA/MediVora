using MediVora.Common;
using MediVora.Data.EF;
using MediVora.DTO;
using MediVora.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediVora.Services
{
    public class UserService : IUserService
    {
        public readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        #region GetAllUse
        public async Task<ApiResponse<List<UserResponseDTO>>> GetAllUsersAsync()
        {
            var vUserList = await _db.Users.AsNoTracking()
                .Where(x => x.IsActive)
                .Select(user => new UserResponseDTO
                {
                    UserID = user.UserID,
                    RoleID = user.RoleID,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    MobileNo = user.MobileNo,
                    IsActive = user.IsActive,
                    LastLoginDate = user.LastLoginDate
                })
                .ToListAsync();

            return ApiResponse<List<UserResponseDTO>>.SuccessResponse(
                vUserList,
                "User list retrieved successfully."
            );
        }
        #endregion


        #region GetAllUsersByRoleAsync
        public async Task<ApiResponse<List<UserResponseDTO>>> GetAllUsersByRoleAsync(int roleId)
        {
            var vUserList = await _db.Users.AsNoTracking()
                .Where(x => x.IsActive && x.RoleID == roleId)
                .Select(user => new UserResponseDTO
                {
                    UserID = user.UserID,
                    RoleID = user.RoleID,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    MobileNo = user.MobileNo,
                    IsActive = user.IsActive,
                    LastLoginDate = user.LastLoginDate
                })
                .ToListAsync();

            return ApiResponse<List<UserResponseDTO>>.SuccessResponse(
                vUserList,
                "User list retrieved successfully."
            );
        }
        #endregion


        #region GetUserByEmailAsync
        public async Task<ApiResponse<UserResponseDTO>> GetUserByEmailAsync(string email)
        {
            var vUser = await _db.Users.AsNoTracking()
                .Where(x => x.Email == email)
                .Select(user => new UserResponseDTO
                {
                    UserID = user.UserID,
                    RoleID = user.RoleID,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    MobileNo = user.MobileNo,
                    IsActive = user.IsActive,
                    LastLoginDate = user.LastLoginDate
                })
                .FirstOrDefaultAsync();

            if (vUser == null)
            {
                return ApiResponse<UserResponseDTO>.ErrorResponse(
                        "User not found.",
                        new List<string> { $"No user found with email: {email}" }
                    );
            }

            return ApiResponse<UserResponseDTO>.SuccessResponse(
                vUser,
                "User retrieved successfully."
            );
        }
        #endregion


        #region GetUserByIdAsync
        public async Task<ApiResponse<UserResponseDTO>> GetUserByIdAsync(int userId)
        {
            var vUser = await _db.Users.AsNoTracking()
                .Where(x => x.UserID == userId)
                .Select(user => new UserResponseDTO
                {
                    UserID = user.UserID,
                    RoleID = user.RoleID,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    MobileNo = user.MobileNo,
                    IsActive = user.IsActive,
                    LastLoginDate = user.LastLoginDate
                })
                .FirstOrDefaultAsync();

            if (vUser == null)
            {
                return ApiResponse<UserResponseDTO>.ErrorResponse(
                        "User not found.",
                        new List<string> { $"No user found with ID: {userId}" }
                    );
            }
            return ApiResponse<UserResponseDTO>.SuccessResponse(
                vUser,
                "User retrieved successfully."
            );
        }
        #endregion
    }

}
