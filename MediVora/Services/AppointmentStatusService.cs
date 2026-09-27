using MediVora.Common;
using MediVora.Data.EF;
using MediVora.DTO;
using MediVora.Entities;
using MediVora.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediVora.Services
{
    public class AppointmentStatusService : IAppointmentStatusService
    {
        public readonly AppDbContext _db;
        public readonly ICurrentUserService _currentUserService;

        public AppointmentStatusService(AppDbContext db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<int>> CreateAppointmentStatusAsync(AppointmentStatusAddEditDTO d)
        {
            var vAppointmentStatus = new AppointmentStatus
            {
                StatusCode = d.StatusCode,
                StatusName = d.StatusName,
                IsActive = d.IsActive,
                CreatedBy = _currentUserService.UserId,
                ModifiedBy = _currentUserService.UserId
            };

            await _db.AppointmentStatus.AddAsync(vAppointmentStatus);
            await _db.SaveChangesAsync();

            return ApiResponse<int>.SuccessResponse(vAppointmentStatus.AppointmentStatusID, "Record Created Successfully.!");
        }

        public async Task<ApiResponse<bool>> UpdateAppointmentStatusAsync(
            int AppointmentStatusID,
            AppointmentStatusAddEditDTO d)
        {
            var vAppointmentStatus = await _db.AppointmentStatus
                .FirstOrDefaultAsync(x => x.AppointmentStatusID == AppointmentStatusID);

            if (vAppointmentStatus == null)
            {
                return ApiResponse<bool>.ErrorResponse("Record Not Found.!");
            }

            vAppointmentStatus.StatusCode = d.StatusCode;
            vAppointmentStatus.StatusName = d.StatusName;
            vAppointmentStatus.IsActive = d.IsActive;
            vAppointmentStatus.ModifiedBy = _currentUserService.UserId;

            await _db.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Record Updated Successfully.!");
        }

        public async Task<ApiResponse<bool>> DeleteAppointmentStatusAsync(int AppointmentStatusID)
        {
            var vAppointmentStatus = await _db.AppointmentStatus.FindAsync(AppointmentStatusID);

            if (vAppointmentStatus == null)
            {
                return ApiResponse<bool>.ErrorResponse("Record Not Found.!");
            }

            _db.AppointmentStatus.Remove(vAppointmentStatus);
            await _db.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Record Deleted Successfully.!");
        }


        public async Task<ApiResponse<AppointmentStatusResponseDTO>> GetAppointmentStatusByIdAsync(
            int AppointmentStatusID)
        {
            var vAppointmentStatus = await _db.AppointmentStatus
                .FirstOrDefaultAsync(x => x.AppointmentStatusID == AppointmentStatusID);

            if (vAppointmentStatus == null)
            {
                return ApiResponse<AppointmentStatusResponseDTO>.ErrorResponse(
                    "Record Not Found.!");
            }

            var vResponse = new AppointmentStatusResponseDTO
            {
                AppointmentStatusID = vAppointmentStatus.AppointmentStatusID,
                StatusCode = vAppointmentStatus.StatusCode,
                StatusName = vAppointmentStatus.StatusName,
                IsActive = vAppointmentStatus.IsActive,
                CreatedBy = vAppointmentStatus.CreatedBy,
                ModifiedBy = vAppointmentStatus.ModifiedBy,
                Created = vAppointmentStatus.Created,
                Modified = vAppointmentStatus.Modified
            };

            return ApiResponse<AppointmentStatusResponseDTO>.SuccessResponse(
                vResponse,
                "Record Retrived Successfully.!");
        }

        public async Task<ApiResponse<List<AppointmentStatusResponseDTO>>> GetAllAppointmentStatusAsync()
        {
            var vAppointmentStatus = await _db.AppointmentStatus
                .ToListAsync();

            var vResponse = vAppointmentStatus.Select(x => new AppointmentStatusResponseDTO
            {
                AppointmentStatusID = x.AppointmentStatusID,
                StatusCode = x.StatusCode,
                StatusName = x.StatusName,
                IsActive = x.IsActive,
                CreatedBy = x.CreatedBy,
                ModifiedBy = x.ModifiedBy,
                Created = x.Created,
                Modified = x.Modified
            }).ToList();

            return ApiResponse<List<AppointmentStatusResponseDTO>>.SuccessResponse(
                vResponse,
                "Records Retrived Successfully.!");
        }

        public async Task<ApiResponse<List<AppointmentStatusDropdownDTO>>> GetAppointmentStatusDropdownAsync()
        {
            var vAppointmentStatus = await _db.AppointmentStatus
                .Where(x => x.IsActive)
                .ToListAsync();

            var vResponse = vAppointmentStatus.Select(x => new AppointmentStatusDropdownDTO
            {
                AppointmentStatusID = x.AppointmentStatusID,
                StatusCode = x.StatusCode,
                StatusName = x.StatusName,
                StatusDisplayName = x.StatusCode + " - " + x.StatusName
            }).ToList();

            return ApiResponse<List<AppointmentStatusDropdownDTO>>.SuccessResponse(
                vResponse,
                "Dropdown Retrived Successfully.!");
        }
    }
}
