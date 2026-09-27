using MediVora.Common;
using MediVora.Data.EF;
using MediVora.DTO;
using MediVora.Entities;
using MediVora.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediVora.Services
{
    public class DoctorScheduleTemplateService : IDoctorScheduleTemplateService
    {
        public readonly AppDbContext _db;
        public readonly ICurrentUserService _currentUserService;
        public DoctorScheduleTemplateService(AppDbContext db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        #region CreateDoctorScheduleTemplateAsync
        public async Task<ApiResponse<int>> CreateDoctorScheduleTemplateAsync(int DoctorID, DoctorScheduleTemplateAddEditDTO d)
        {
            if (DoctorID <= 0)
            {
                return ApiResponse<int>.ErrorResponse("Invalid doctor ID");
            }

            if (!await _db.Doctors.AnyAsync(d => d.DoctorID == DoctorID && !d.IsTrashed && d.IsActive))
            {
                return ApiResponse<int>.ErrorResponse("Doctor not found");
            }

            var vDoctorScheduleTemplate = new DoctorScheduleTemplate
            {
                DoctorID = DoctorID,
                DayOfWeek = d.DayOfWeek,
                DayOfWeekName = d.DayOfWeekName,
                StartTime = d.StartTime,
                EndTime = d.EndTime,
                SlotDurationInMinutes = d.SlotDurationInMinutes,
                MaxAppointmentsPerSlot = d.MaxAppointmentsPerSlot,
                EffectiveFromDate = d.EffectiveFromDate,
                EffectiveToDate = d.EffectiveToDate,
                IsActive = d.IsActive,
                CreatedBy = _currentUserService.UserId,
                ModifiedBy = _currentUserService.UserId
            };

            await _db.DoctorScheduleTemplates.AddAsync(vDoctorScheduleTemplate);
            await _db.SaveChangesAsync();

            return ApiResponse<int>.SuccessResponse(vDoctorScheduleTemplate.DoctorScheduleTemplateID, "Doctor schedule template created successfully");
        }
        #endregion


        #region DeleteDoctorScheduleTemplateAsync
        public async Task<ApiResponse<bool>> DeleteDoctorScheduleTemplateAsync(int DoctorID, int id)
        {
            if (DoctorID <= 0)
            {
                return ApiResponse<bool>.ErrorResponse("Invalid doctor ID");
            }

            if (!await _db.Doctors.AnyAsync(d => d.DoctorID == DoctorID && !d.IsTrashed && d.IsActive))
            {
                return ApiResponse<bool>.ErrorResponse("Doctor not found");
            }

            var vDoctorScheduleTemplate = await _db.DoctorScheduleTemplates.FirstOrDefaultAsync(d => d.DoctorScheduleTemplateID == id && d.DoctorID == DoctorID);

            if (vDoctorScheduleTemplate == null)
            {
                return ApiResponse<bool>.ErrorResponse("Doctor schedule template not found");
            }

            _db.DoctorScheduleTemplates.Remove(vDoctorScheduleTemplate);
            await _db.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Doctor schedule template deleted successfully");
        }
        #endregion


        #region GetAllDoctorScheduleTemplatesAsync
        public async Task<ApiResponse<List<DoctorScheduleTemplateResponseDTO>>> GetAllDoctorScheduleTemplatesAsync(int DoctorID)
        {
            if (DoctorID <= 0)
            {
                return ApiResponse<List<DoctorScheduleTemplateResponseDTO>>.ErrorResponse("Invalid doctor ID");
            }

            if (!await _db.Doctors.AnyAsync(d => d.DoctorID == DoctorID && !d.IsTrashed && d.IsActive))
            {
                return ApiResponse<List<DoctorScheduleTemplateResponseDTO>>.ErrorResponse("Doctor not found");
            }

            var vDoctorScheduleTemplates = await _db.DoctorScheduleTemplates.AsNoTracking().Include(d => d.Doctor).Where(d => d.IsActive && d.DoctorID == DoctorID).ToListAsync();

            var vDoctorScheduleTemplateResponses = vDoctorScheduleTemplates.Select(d => new DoctorScheduleTemplateResponseDTO
            {
                DoctorScheduleTemplateID = d.DoctorScheduleTemplateID,
                DoctorID = d.DoctorID,
                DoctorName = d.Doctor.FirstName + " " + d.Doctor.MiddleName + " " + d.Doctor.LastName,
                DoctorCode = d.Doctor.DoctorCode,
                MedicalRegistrationNo = d.Doctor.MedicalRegistrationNo,
                ConsultationFee = d.Doctor.ConsultationFee,
                DayOfWeek = d.DayOfWeek,
                DayOfWeekName = d.DayOfWeekName,
                StartTime = d.StartTime,
                EndTime = d.EndTime,
                SlotDurationInMinutes = d.SlotDurationInMinutes,
                MaxAppointmentsPerSlot = d.MaxAppointmentsPerSlot,
                EffectiveFromDate = d.EffectiveFromDate,
                EffectiveToDate = d.EffectiveToDate,
                IsActive = d.IsActive,
                CreatedBy = d.CreatedBy,
                ModifiedBy = d.ModifiedBy,
                Created = d.Created,
                Modified = d.Modified
            }).ToList();

            return ApiResponse<List<DoctorScheduleTemplateResponseDTO>>.SuccessResponse(vDoctorScheduleTemplateResponses, "Doctor schedule templates retrieved successfully");
        }
        #endregion


        #region GetDoctorScheduleTemplateByIdAsync
        public async Task<ApiResponse<DoctorScheduleTemplateResponseDTO>> GetDoctorScheduleTemplateByIdAsync(int DoctorID, int id)
        {
            if (DoctorID <= 0)
            {
                return ApiResponse<DoctorScheduleTemplateResponseDTO>.ErrorResponse("Invalid doctor ID");
            }

            if (!await _db.Doctors.AnyAsync(d => d.DoctorID == DoctorID && !d.IsTrashed && d.IsActive))
            {
                return ApiResponse<DoctorScheduleTemplateResponseDTO>.ErrorResponse("Doctor not found");
            }

            var vDoctorScheduleTemplate = await _db.DoctorScheduleTemplates.AsNoTracking().Include(d => d.Doctor).FirstOrDefaultAsync(d => d.DoctorScheduleTemplateID == id && d.DoctorID == DoctorID);

            if (vDoctorScheduleTemplate == null)
            {
                return ApiResponse<DoctorScheduleTemplateResponseDTO>.ErrorResponse("Doctor schedule template not found");
            }

            var vDoctorScheduleTemplateResponse = new DoctorScheduleTemplateResponseDTO
            {
                DoctorScheduleTemplateID = vDoctorScheduleTemplate.DoctorScheduleTemplateID,
                DoctorID = vDoctorScheduleTemplate.DoctorID,
                DoctorName = vDoctorScheduleTemplate.Doctor.FirstName + " " + vDoctorScheduleTemplate.Doctor.MiddleName + " " + vDoctorScheduleTemplate.Doctor.LastName,
                DoctorCode = vDoctorScheduleTemplate.Doctor.DoctorCode,
                MedicalRegistrationNo = vDoctorScheduleTemplate.Doctor.MedicalRegistrationNo,
                ConsultationFee = vDoctorScheduleTemplate.Doctor.ConsultationFee,
                DayOfWeek = vDoctorScheduleTemplate.DayOfWeek,
                DayOfWeekName = vDoctorScheduleTemplate.DayOfWeekName,
                StartTime = vDoctorScheduleTemplate.StartTime,
                EndTime = vDoctorScheduleTemplate.EndTime,
                SlotDurationInMinutes = vDoctorScheduleTemplate.SlotDurationInMinutes,
                MaxAppointmentsPerSlot = vDoctorScheduleTemplate.MaxAppointmentsPerSlot,
                EffectiveFromDate = vDoctorScheduleTemplate.EffectiveFromDate,
                EffectiveToDate = vDoctorScheduleTemplate.EffectiveToDate,
                IsActive = vDoctorScheduleTemplate.IsActive,
                CreatedBy = vDoctorScheduleTemplate.CreatedBy,
                ModifiedBy = vDoctorScheduleTemplate.ModifiedBy,
                Created = vDoctorScheduleTemplate.Created,
                Modified = vDoctorScheduleTemplate.Modified
            };

            return ApiResponse<DoctorScheduleTemplateResponseDTO>.SuccessResponse(vDoctorScheduleTemplateResponse, "Doctor schedule template retrieved successfully");
        }
        #endregion


        #region UpdateDoctorScheduleTemplateAsync
        public async Task<ApiResponse<bool>> UpdateDoctorScheduleTemplateAsync(int DoctorID, int id, DoctorScheduleTemplateAddEditDTO d)
        {
            if (DoctorID <= 0)
            {
                return ApiResponse<bool>.ErrorResponse("Invalid doctor ID");
            }

            if (!await _db.Doctors.AnyAsync(d => d.DoctorID == DoctorID && !d.IsTrashed && d.IsActive))
            {
                return ApiResponse<bool>.ErrorResponse("Doctor not found");
            }

            var vDoctorScheduleTemplate = await _db.DoctorScheduleTemplates.FindAsync(id);

            if (vDoctorScheduleTemplate == null)
            {
                return ApiResponse<bool>.ErrorResponse("Doctor schedule template not found");
            }

            vDoctorScheduleTemplate.DoctorID = DoctorID;
            vDoctorScheduleTemplate.DayOfWeek = d.DayOfWeek;
            vDoctorScheduleTemplate.DayOfWeekName = d.DayOfWeekName;
            vDoctorScheduleTemplate.StartTime = d.StartTime;
            vDoctorScheduleTemplate.EndTime = d.EndTime;
            vDoctorScheduleTemplate.SlotDurationInMinutes = d.SlotDurationInMinutes;
            vDoctorScheduleTemplate.MaxAppointmentsPerSlot = d.MaxAppointmentsPerSlot;
            vDoctorScheduleTemplate.EffectiveFromDate = d.EffectiveFromDate;
            vDoctorScheduleTemplate.EffectiveToDate = d.EffectiveToDate;
            vDoctorScheduleTemplate.IsActive = d.IsActive;
            vDoctorScheduleTemplate.ModifiedBy = _currentUserService.UserId;

            await _db.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Doctor schedule template updated successfully");
        }
        #endregion


        #region UpdateDoctorScheduleTemplateStatusAsync
        public async Task<ApiResponse<bool>> UpdateDoctorScheduleTemplateStatusAsync(int DoctorID, int id, bool IsActive)
        {
            if (DoctorID <= 0)
            {
                return ApiResponse<bool>.ErrorResponse("Invalid doctor ID");
            }

            if (!await _db.Doctors.AnyAsync(d => d.DoctorID == DoctorID && !d.IsTrashed && d.IsActive))
            {
                return ApiResponse<bool>.ErrorResponse("Doctor not found");
            }

            var vDoctorScheduleTemplate = await _db.DoctorScheduleTemplates.FindAsync(id);

            if (vDoctorScheduleTemplate == null)
            {
                return ApiResponse<bool>.ErrorResponse("Doctor schedule template not found");
            }

            vDoctorScheduleTemplate.IsActive = IsActive;
            vDoctorScheduleTemplate.ModifiedBy = _currentUserService.UserId;

            await _db.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Doctor schedule template status updated successfully");
        }
        #endregion
    }
}
