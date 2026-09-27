using MediVora.Common;
using MediVora.Data.EF;
using MediVora.DTO;
using MediVora.Entities;
using MediVora.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediVora.Services
{
    public class DoctorScheduleService : IDoctorScheduleService
    {
        public readonly AppDbContext _db;
        public readonly ICurrentUserService _currentUserService;
        public DoctorScheduleService(AppDbContext db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        #region CreateDoctorScheduleAsync
        public async Task<ApiResponse<int>> CreateDoctorScheduleAsync(int DoctorID, DoctorScheduleAddEditDTO d)
        {
            if (DoctorID <= 0)
            {
                return ApiResponse<int>.ErrorResponse("Invalid doctor ID");
            }

            if (!await _db.Doctors.AnyAsync(d => d.DoctorID == DoctorID && !d.IsTrashed && d.IsActive))
            {
                return ApiResponse<int>.ErrorResponse("Doctor not found");
            }

            var vDoctorSchedule = new DoctorSchedule
            {
                DoctorID = DoctorID,
                ScheduleDate = d.ScheduleDate,
                StartTime = d.StartTime,
                EndTime = d.EndTime,
                MaxAppointments = d.MaxAppointments,
                IsAvailable = d.IsAvailable,
                Remarks = d.Remarks,
                CreatedBy = _currentUserService.UserId,
                ModifiedBy = _currentUserService.UserId,
            };

            await _db.DoctorSchedules.AddAsync(vDoctorSchedule);
            await _db.SaveChangesAsync();

            return ApiResponse<int>.SuccessResponse(vDoctorSchedule.DoctorScheduleID, "Doctor schedule created successfully");
        }
        #endregion


        #region DeleteDoctorScheduleAsync
        public async Task<ApiResponse<bool>> DeleteDoctorScheduleAsync(int DoctorID, int id)
        {
            if (DoctorID <= 0)
            {
                return ApiResponse<bool>.ErrorResponse("Invalid doctor ID");
            }

            if (!await _db.Doctors.AnyAsync(d => d.DoctorID == DoctorID && !d.IsTrashed && d.IsActive))
            {
                return ApiResponse<bool>.ErrorResponse("Doctor not found");
            }

            var vDoctorSchedule = await _db.DoctorSchedules.FirstOrDefaultAsync(d => d.DoctorScheduleID == id && d.DoctorID == DoctorID);

            if (vDoctorSchedule == null)
            {
                return ApiResponse<bool>.ErrorResponse("Doctor schedule not found");
            }

            _db.DoctorSchedules.Remove(vDoctorSchedule);
            await _db.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Doctor schedule deleted successfully");
        }
        #endregion


        #region GenerateDoctorScheduleAsync
        public async Task<ApiResponse<bool>> GenerateDoctorScheduleAsync(int DoctorID, GenerateDoctorScheduleDTO d)
        {
            if (DoctorID <= 0)
            {
                return ApiResponse<bool>.ErrorResponse("Invalid doctor ID");
            }

            if (!await _db.Doctors.AnyAsync(d => d.DoctorID == DoctorID && !d.IsTrashed && d.IsActive))
            {
                return ApiResponse<bool>.ErrorResponse("Doctor not found");
            }

            var vTemplates = await _db.DoctorScheduleTemplates.Where(x =>
                                        x.DoctorID == DoctorID && x.IsActive && x.EffectiveFromDate <= d.ToDate.Date && (x.EffectiveToDate == null || x.EffectiveToDate >= d.FromDate.Date)).ToListAsync();

            if (vTemplates == null)
            {
                return ApiResponse<bool>.ErrorResponse("No active schedule template found for this doctor.");
            }

            var vExistingSchedules = await _db.DoctorSchedules.Where(s => s.DoctorID == DoctorID &&
                                                s.ScheduleDate >= d.FromDate.Date && s.ScheduleDate <= d.ToDate.Date).ToListAsync();

            var vSchedulesToInsert = new List<DoctorSchedule>();

            #region Generate schedules based on templates
            for (var date = d.FromDate.Date; date <= d.ToDate.Date; date = date.AddDays(1))
            {
                // Convert C# DayOfWeek to your 1-7 format
                // Monday = 1 ... Sunday = 7
                int dayOfWeek = date.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)date.DayOfWeek;

                var vDayTemplates = vTemplates.Where(t => t.DayOfWeek == dayOfWeek &&
                                                t.EffectiveFromDate <= date && (t.EffectiveToDate == null || t.EffectiveToDate >= date)).ToList();

                // No template = doctor doesn't work that day
                if (!vDayTemplates.Any())
                {
                    continue;
                }

                // Generate slots
                foreach (var vTemplate in vDayTemplates)
                {
                    var currentTime = vTemplate.StartTime;
                    var endTime = vTemplate.EndTime;

                    while (currentTime < endTime)
                    {
                        var slotEndTime = currentTime.Add(TimeSpan.FromMinutes(vTemplate.SlotDurationInMinutes));

                        // Don't create slot beyond working time
                        if (slotEndTime > endTime)
                        {
                            break;
                        }

                        // Prevent duplicate schedule
                        bool vAlreadyExists = vExistingSchedules.Any(s =>
                            s.ScheduleDate == date &&
                            s.StartTime == currentTime &&
                            s.EndTime == slotEndTime);

                        bool vAlreadyAdded = vSchedulesToInsert.Any(s =>
                            s.ScheduleDate == date &&
                            s.StartTime == currentTime &&
                            s.EndTime == slotEndTime);

                        if (!vAlreadyExists && !vAlreadyAdded)
                        {
                            vSchedulesToInsert.Add(new DoctorSchedule
                            {
                                DoctorID = DoctorID,
                                ScheduleDate = date,
                                StartTime = currentTime,
                                EndTime = slotEndTime,
                                MaxAppointments = vTemplate.MaxAppointmentsPerSlot,
                                IsAvailable = true,
                                Remarks = null,
                                CreatedBy = _currentUserService.UserId,
                                ModifiedBy = _currentUserService.UserId,
                            });
                        }

                        currentTime = slotEndTime;
                    }
                }

                if (vSchedulesToInsert.Any())
                {
                    await _db.DoctorSchedules.AddRangeAsync(vSchedulesToInsert);
                    await _db.SaveChangesAsync();
                }

            }
            #endregion

            return ApiResponse<bool>.SuccessResponse(true, $"{vSchedulesToInsert.Count} schedules generated successfully.");
        }
        #endregion


        #region GetDoctorScheduleByIdAsync
        public async Task<ApiResponse<DoctorScheduleResponseDTO>> GetDoctorScheduleByIdAsync(int DoctorID, int id)
        {
            if (DoctorID <= 0)
            {
                return ApiResponse<DoctorScheduleResponseDTO>.ErrorResponse("Invalid doctor ID");
            }

            if (!await _db.Doctors.AnyAsync(d => d.DoctorID == DoctorID && !d.IsTrashed && d.IsActive))
            {
                return ApiResponse<DoctorScheduleResponseDTO>.ErrorResponse("Doctor not found");
            }

            var vDoctorSchedule = await _db.DoctorSchedules.AsNoTracking().Include(d => d.Doctor)
                .Where(d => d.DoctorScheduleID == id && d.DoctorID == DoctorID)
                .Select(d => new DoctorScheduleResponseDTO
                {
                    DoctorScheduleID = d.DoctorScheduleID,
                    DoctorID = d.DoctorID,
                    DoctorName = d.Doctor.FirstName + " " + d.Doctor.MiddleName + " " + d.Doctor.LastName,
                    DoctorCode = d.Doctor.DoctorCode,
                    MedicalRegistrationNo = d.Doctor.MedicalRegistrationNo,
                    ConsultationFee = d.Doctor.ConsultationFee,
                    ScheduleDate = d.ScheduleDate,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                    MaxAppointments = d.MaxAppointments,
                    IsAvailable = d.IsAvailable,
                    Remarks = d.Remarks,
                    CreatedBy = d.CreatedBy,
                    ModifiedBy = d.ModifiedBy,
                    Created = d.Created,
                    Modified = d.Modified
                })
                .FirstOrDefaultAsync();

            if (vDoctorSchedule == null)
            {
                return ApiResponse<DoctorScheduleResponseDTO>.ErrorResponse("Schedule not found");
            }

            return ApiResponse<DoctorScheduleResponseDTO>.SuccessResponse(vDoctorSchedule, "Schedule retrieved successfully.");
        }
        #endregion


        #region  GetDoctorSchedulesAsync
        public async Task<ApiResponse<List<DoctorScheduleResponseDTO>>> GetDoctorSchedulesAsync(int? DoctorID = null)
        {
            int vDoctorID;

            // Admin -> DoctorID comes from request
            if (_currentUserService.IsAdmin == true)
            {
                if (!DoctorID.HasValue || DoctorID.Value <= 0)
                {
                    return ApiResponse<List<DoctorScheduleResponseDTO>>.ErrorResponse("Invalid doctor ID");
                }

                vDoctorID = DoctorID.Value;
            }
            // Doctor -> DoctorID comes from logged-in user
            else if (_currentUserService.IsDoctor == true)
            {
                int UserId = _currentUserService.UserId;

                vDoctorID = _db.Doctors.Where(d => d.UserID == UserId && !d.IsTrashed && d.IsActive).Select(d => d.DoctorID).FirstOrDefault();

                if (vDoctorID <= 0)
                {
                    return ApiResponse<List<DoctorScheduleResponseDTO>>
                        .ErrorResponse("Doctor information not found for logged-in user");
                }
            }
            else
            {
                return ApiResponse<List<DoctorScheduleResponseDTO>>
                    .ErrorResponse("You are not authorized to view doctor schedules");
            }

            var doctorExists = await _db.Doctors.AnyAsync(d => d.DoctorID == vDoctorID && !d.IsTrashed && d.IsActive);

            if (!doctorExists)
            {
                return ApiResponse<List<DoctorScheduleResponseDTO>>.ErrorResponse("Doctor not found");
            }


            var vDoctorSchedules = await _db.DoctorSchedules.AsNoTracking().Include(d => d.Doctor)
                .Where(d => d.DoctorID == vDoctorID)
                .Select(d => new DoctorScheduleResponseDTO
                {
                    DoctorScheduleID = d.DoctorScheduleID,
                    DoctorID = d.DoctorID,
                    DoctorName = d.Doctor.FirstName + " " + d.Doctor.MiddleName + " " + d.Doctor.LastName,
                    DoctorCode = d.Doctor.DoctorCode,
                    MedicalRegistrationNo = d.Doctor.MedicalRegistrationNo,
                    ConsultationFee = d.Doctor.ConsultationFee,
                    ScheduleDate = d.ScheduleDate,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                    MaxAppointments = d.MaxAppointments,
                    IsAvailable = d.IsAvailable,
                    Remarks = d.Remarks,
                    CreatedBy = d.CreatedBy,
                    ModifiedBy = d.ModifiedBy,
                    Created = d.Created,
                    Modified = d.Modified
                })
                .ToListAsync();

            return ApiResponse<List<DoctorScheduleResponseDTO>>.SuccessResponse(vDoctorSchedules, "Schedules retrieved successfully.");
        }
        #endregion


        #region UpdateDoctorScheduleAsync
        public async Task<ApiResponse<bool>> UpdateDoctorScheduleAsync(int DoctorID, int id, DoctorScheduleAddEditDTO d)
        {
            if (DoctorID <= 0)
            {
                return ApiResponse<bool>.ErrorResponse("Invalid doctor ID");
            }

            if (!await _db.Doctors.AnyAsync(d => d.DoctorID == DoctorID && !d.IsTrashed && d.IsActive))
            {
                return ApiResponse<bool>.ErrorResponse("Doctor not found");
            }

            var vDoctorSchedule = await _db.DoctorSchedules.FirstOrDefaultAsync(d => d.DoctorScheduleID == id && d.DoctorID == DoctorID);

            if (vDoctorSchedule == null)
            {
                return ApiResponse<bool>.ErrorResponse("Doctor schedule not found");
            }

            vDoctorSchedule.ScheduleDate = d.ScheduleDate;
            vDoctorSchedule.StartTime = d.StartTime;
            vDoctorSchedule.EndTime = d.EndTime;
            vDoctorSchedule.MaxAppointments = d.MaxAppointments;
            vDoctorSchedule.Remarks = d.Remarks;
            vDoctorSchedule.IsAvailable = d.IsAvailable;
            vDoctorSchedule.ModifiedBy = _currentUserService.UserId;

            await _db.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Doctor schedule updated successfully");
        }
        #endregion


        #region UpdateDoctorScheduleStatusAsync
        public async Task<ApiResponse<bool>> UpdateDoctorScheduleStatusAsync(int DoctorID, int id, bool IsAvailable)
        {
            if (DoctorID <= 0)
            {
                return ApiResponse<bool>.ErrorResponse("Invalid doctor ID");
            }

            if (!await _db.Doctors.AnyAsync(d => d.DoctorID == DoctorID && !d.IsTrashed && d.IsActive))
            {
                return ApiResponse<bool>.ErrorResponse("Doctor not found");
            }

            var vDoctorSchedule = await _db.DoctorSchedules.FirstOrDefaultAsync(d => d.DoctorScheduleID == id && d.DoctorID == DoctorID);

            if (vDoctorSchedule == null)
            {
                return ApiResponse<bool>.ErrorResponse("Doctor schedule not found");
            }

            vDoctorSchedule.IsAvailable = IsAvailable;

            return ApiResponse<bool>.SuccessResponse(true, "Doctor schedule status updated successfully");
        }
        #endregion


        #region GetDoctorSchedulesByDateRangeAsync
        public async Task<ApiResponse<List<DoctorScheduleResponseDTO>>> GetDoctorSchedulesByDateRangeAsync(DateTime startDate, DateTime endDate, int? DoctorID = null)
        {
            // Validate date range
            if (startDate.Date > endDate.Date)
            {
                return ApiResponse<List<DoctorScheduleResponseDTO>>.ErrorResponse("Start date cannot be greater than end date.");
            }

            int vDoctorID;

            if (_currentUserService.IsDoctor == true)
            {
                int UserID = _currentUserService.UserId;

                vDoctorID = await _db.Doctors.Where(d => d.UserID == UserID && !d.IsTrashed && d.IsActive).Select(d => d.DoctorID).FirstOrDefaultAsync();

                if (vDoctorID <= 0)
                {
                    return ApiResponse<List<DoctorScheduleResponseDTO>>.ErrorResponse("Doctor information not found for logged-in user.");
                }
            }
            else
            {
                if (!DoctorID.HasValue || DoctorID.Value <= 0)
                {
                    return ApiResponse<List<DoctorScheduleResponseDTO>>.ErrorResponse("Invalid doctor ID.");
                }

                vDoctorID = DoctorID.Value;

                // Verify doctor exists and is active
                bool vDoctorExists = await _db.Doctors.AnyAsync(d => d.DoctorID == vDoctorID && !d.IsTrashed && d.IsActive);

                if (!vDoctorExists)
                {
                    return ApiResponse<List<DoctorScheduleResponseDTO>>.ErrorResponse("Doctor not found.");
                }
            }

            var vQuery = await _db.DoctorSchedules.AsNoTracking().Include(d => d.Doctor)
                .Where(d => d.DoctorID == vDoctorID && d.ScheduleDate.Date >= startDate.Date && d.ScheduleDate.Date <= endDate.Date)
                .Select(d => new DoctorScheduleResponseDTO
                {
                    DoctorScheduleID = d.DoctorScheduleID,
                    DoctorID = d.DoctorID,
                    DoctorName = d.Doctor.FirstName + " " + d.Doctor.MiddleName + " " + d.Doctor.LastName,
                    DoctorCode = d.Doctor.DoctorCode,
                    MedicalRegistrationNo = d.Doctor.MedicalRegistrationNo,
                    ConsultationFee = d.Doctor.ConsultationFee,
                    ScheduleDate = d.ScheduleDate,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                    MaxAppointments = d.MaxAppointments,
                    IsAvailable = d.IsAvailable,
                    Remarks = d.Remarks,
                    CreatedBy = d.CreatedBy,
                    ModifiedBy = d.ModifiedBy,
                    Created = d.Created,
                    Modified = d.Modified
                }).ToListAsync();

            return ApiResponse<List<DoctorScheduleResponseDTO>>.SuccessResponse(vQuery, "Doctor schedules retrieved successfully");
        }
        #endregion
    }
}
