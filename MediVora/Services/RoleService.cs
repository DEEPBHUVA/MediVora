using MediVora.Common;
using MediVora.Data.EF;
using MediVora.DTO;
using MediVora.Entities;
using MediVora.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediVora.Services
{
    public class RoleService : IRoleService
    {
        public readonly AppDbContext _db;
        private readonly ICurrentUserService _currentUser;

        public RoleService(AppDbContext db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        #region CreateRoleAsync
        public async Task<ApiResponse<RoleResponseDTO>> CreateRoleAsync(RoleAddEditDTO d)
        {
            var userId = _currentUser.UserId;

            var role = new Roles
            {
                RoleCode = d.RoleCode,
                RoleName = d.RoleName,
                RoleDescription = d.RoleDescription,
                IsActive = d.IsActive,
                CreatedBy = userId,
                ModifiedBy = userId,
            };

            await _db.Roles.AddAsync(role);
            await _db.SaveChangesAsync();

            var response = new RoleResponseDTO
            {
                RoleID = role.RoleID,
                RoleCode = role.RoleCode,
                RoleName = role.RoleName,
                RoleDescription = role.RoleDescription,
                IsActive = role.IsActive,
                CreatedBy = role.CreatedBy,
                ModifiedBy = role.ModifiedBy,
                Created = role.Created,
                Modified = role.Modified
            };

            return ApiResponse<RoleResponseDTO>.SuccessResponse(response, "Role created successfully.");
        }
        #endregion


        #region DeleteRoleAsync
        public async Task<ApiResponse<bool>> DeleteRoleAsync(int roleId)
        {
            var role = await _db.Roles.FindAsync(roleId);

            if (role == null)
            {
                return ApiResponse<bool>.ErrorResponse("Role not found.");
            }

            _db.Roles.Remove(role);
            await _db.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Role deleted successfully.");
        }
        #endregion


        #region GetAllRolesAsync
        public async Task<ApiResponse<List<RoleResponseDTO>>> GetAllRolesAsync()
        {
            var vRoleList = await _db.Roles.AsNoTracking().Where(x => x.IsActive == true).ToListAsync();

            var vResponse = vRoleList.Select(r => new RoleResponseDTO
            {
                RoleID = r.RoleID,
                RoleCode = r.RoleCode,
                RoleName = r.RoleName,
                RoleDescription = r.RoleDescription,
                IsActive = r.IsActive,
                CreatedBy = r.CreatedBy,
                ModifiedBy = r.ModifiedBy,
                Created = r.Created,
                Modified = r.Modified
            }).ToList();

            return ApiResponse<List<RoleResponseDTO>>.SuccessResponse(vResponse, "Roles retrieved successfully.");
        }
        #endregion


        #region GetRoleByIdAsync
        public async Task<ApiResponse<RoleResponseDTO>> GetRoleByIdAsync(int roleId)
        {
            var role = await _db.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.RoleID == roleId);

            if (role == null)
            {
                return ApiResponse<RoleResponseDTO>.ErrorResponse("Role not found.");
            }

            var response = new RoleResponseDTO
            {
                RoleID = role.RoleID,
                RoleCode = role.RoleCode,
                RoleName = role.RoleName,
                RoleDescription = role.RoleDescription,
                IsActive = role.IsActive,
                CreatedBy = role.CreatedBy,
                ModifiedBy = role.ModifiedBy,
                Created = role.Created,
                Modified = role.Modified
            };

            return ApiResponse<RoleResponseDTO>.SuccessResponse(response, "Role retrieved successfully.");
        }
        #endregion


        #region UpdateRoleAsync
        public async Task<ApiResponse<RoleResponseDTO>> UpdateRoleAsync(int roleId, RoleAddEditDTO d)
        {
            var vRole = await _db.Roles.FindAsync(roleId);
            if (vRole == null)
            {
                return ApiResponse<RoleResponseDTO>.ErrorResponse("Role not found.");
            }

            vRole.RoleCode = d.RoleCode;
            vRole.RoleName = d.RoleName;
            vRole.RoleDescription = d.RoleDescription;
            vRole.IsActive = d.IsActive;
            vRole.ModifiedBy = _currentUser.UserId;

            await _db.SaveChangesAsync();

            var response = new RoleResponseDTO
            {
                RoleID = vRole.RoleID,
                RoleCode = vRole.RoleCode,
                RoleName = vRole.RoleName,
                RoleDescription = vRole.RoleDescription,
                IsActive = vRole.IsActive,
                CreatedBy = vRole.CreatedBy,
                ModifiedBy = vRole.ModifiedBy,
                Created = vRole.Created,
                Modified = vRole.Modified
            };
            return ApiResponse<RoleResponseDTO>.SuccessResponse(response, "Role updated successfully.");
        }
        #endregion
    }
}
