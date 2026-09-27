using MediVora.Common;
using MediVora.Data.EF;
using MediVora.DTO;
using MediVora.Entities;
using MediVora.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediVora.Services
{
    public class DepartmentService : IDepartmentService
    {
        public readonly AppDbContext _db;
        public readonly ICurrentUserService _currentUser;

        public DepartmentService(AppDbContext db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        #region CreateDepartmentAsync
        public async Task<ApiResponse<DepartmentResponseDTO>> CreateDepartmentAsync(DepartmentAddEditDTO d)
        {
            var userId = _currentUser.UserId;

            var vDepartment = new Department
            {
                DepartmentCode = d.DepartmentCode,
                DepartmentName = d.DepartmentName,
                Description = d.Description,
                IsActive = d.IsActive,
                CreatedBy = userId,
                ModifiedBy = userId,
            };

            await _db.Departments.AddAsync(vDepartment);
            await _db.SaveChangesAsync();

            var vResponse = new DepartmentResponseDTO
            {
                DepartmentId = vDepartment.DepartmentId,
                DepartmentCode = vDepartment.DepartmentCode,
                DepartmentName = vDepartment.DepartmentName,
                Description = vDepartment.Description,
                IsActive = vDepartment.IsActive,
                CreatedBy = vDepartment.CreatedBy,
                ModifiedBy = vDepartment.ModifiedBy,
                Created = vDepartment.Created,
                Modified = vDepartment.Modified,
            };

            return ApiResponse<DepartmentResponseDTO>.SuccessResponse(vResponse, "Record Retrived Successfully.!");
        }
        #endregion


        #region DeleteDepartmentAsync
        public async Task<ApiResponse<bool>> DeleteDepartmentAsync(int departmentId)
        {
            if (departmentId == null)
            {
                return ApiResponse<bool>.ErrorResponse("Department Id is required.!");
            }

            var vDepartment = await _db.Departments.FindAsync(departmentId);
            if (vDepartment == null)
            {
                return ApiResponse<bool>.ErrorResponse("Department not found.!");
            }

            _db.Departments.Remove(vDepartment);
            await _db.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Department deleted successfully.!");
        }
        #endregion


        #region GetAllDepartmentsAsync
        public async Task<ApiResponse<List<DepartmentResponseDTO>>> GetAllDepartmentsAsync()
        {
            var vDepartments = await _db.Departments.AsNoTracking().Where(d => d.IsActive == true).ToListAsync();
            var vResponse = vDepartments.Select(d => new DepartmentResponseDTO
            {
                DepartmentId = d.DepartmentId,
                DepartmentCode = d.DepartmentCode,
                DepartmentName = d.DepartmentName,
                Description = d.Description,
                IsActive = d.IsActive,
                CreatedBy = d.CreatedBy,
                ModifiedBy = d.ModifiedBy,
                Created = d.Created,
                Modified = d.Modified,
            }).ToList();

            return ApiResponse<List<DepartmentResponseDTO>>.SuccessResponse(vResponse, "Records retrieved successfully.!");
        }
        #endregion


        #region GetDepartmentByIdAsync
        public async Task<ApiResponse<DepartmentResponseDTO>> GetDepartmentByIdAsync(int departmentId)
        {
            var vDepartment = await _db.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.DepartmentId == departmentId && d.IsActive == true);
            if (vDepartment == null)
            {
                return ApiResponse<DepartmentResponseDTO>.ErrorResponse("Department not found.!");
            }

            var vResponse = new DepartmentResponseDTO
            {
                DepartmentId = vDepartment.DepartmentId,
                DepartmentCode = vDepartment.DepartmentCode,
                DepartmentName = vDepartment.DepartmentName,
                Description = vDepartment.Description,
                IsActive = vDepartment.IsActive,
                CreatedBy = vDepartment.CreatedBy,
                ModifiedBy = vDepartment.ModifiedBy,
                Created = vDepartment.Created,
                Modified = vDepartment.Modified,
            };

            return ApiResponse<DepartmentResponseDTO>.SuccessResponse(vResponse, "Record retrieved successfully.!");
        }
        #endregion


        #region UpdateDepartmentAsync
        public async Task<ApiResponse<DepartmentResponseDTO>> UpdateDepartmentAsync(int departmentId, DepartmentAddEditDTO departmentRequestDTO)
        {
            var vDepartment = await _db.Departments.FindAsync(departmentId);
            if (vDepartment == null)
            {
                return ApiResponse<DepartmentResponseDTO>.ErrorResponse("Department not found.!");
            }

            vDepartment.DepartmentCode = departmentRequestDTO.DepartmentCode;
            vDepartment.DepartmentName = departmentRequestDTO.DepartmentName;
            vDepartment.Description = departmentRequestDTO.Description;
            vDepartment.IsActive = departmentRequestDTO.IsActive;
            vDepartment.ModifiedBy = _currentUser.UserId;

            _db.Departments.Update(vDepartment);
            await _db.SaveChangesAsync();

            var vResponse = new DepartmentResponseDTO
            {
                DepartmentId = vDepartment.DepartmentId,
                DepartmentCode = vDepartment.DepartmentCode,
                DepartmentName = vDepartment.DepartmentName,
                Description = vDepartment.Description,
                IsActive = vDepartment.IsActive,
                CreatedBy = vDepartment.CreatedBy,
                ModifiedBy = vDepartment.ModifiedBy,
                Created = vDepartment.Created,
                Modified = vDepartment.Modified,
            };

            return ApiResponse<DepartmentResponseDTO>.SuccessResponse(vResponse, "Department updated successfully.!");
        }
        #endregion
    }
}
