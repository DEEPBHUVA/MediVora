using MediVora.Common;
using MediVora.Data.EF;
using MediVora.DTO;
using MediVora.Entities;
using MediVora.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediVora.Services
{
    public class AppointmentService : IAppointmentService
    {
        public readonly AppDbContext _db;
        public readonly ICurrentUserService _currentUserService;

        public AppointmentService(AppDbContext db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }


        #region  CreateAppointmentByAdminAsync
        public async Task<ApiResponse<int>> CreateAppointmentByAdminAsync(AdminAppointmentAddDTO d)
        {
            var vDoctorSchedule = await _db.DoctorSchedules.FirstOrDefaultAsync(x => x.DoctorScheduleID == d.DoctorScheduleID && x.IsAvailable == true);

            if (vDoctorSchedule == null)
            {
                return ApiResponse<int>.ErrorResponse("Doctor Schedule Not Found.!");
            }

            var vPatient = await _db.Patients.FirstOrDefaultAsync(x => x.PatientID == d.PatientID);

            if (vPatient == null)
            {
                return ApiResponse<int>.ErrorResponse("Patient Not Found.!");
            }

            var vAppointmentNo = await GenerateAppointmentNoAsync();

            var vAppointment = new Appointment
            {
                AppointmentNo = vAppointmentNo,
                DoctorScheduleID = d.DoctorScheduleID,
                DoctorID = vDoctorSchedule.DoctorID,
                PatientID = d.PatientID,
                AppointmentStatusID = 1,
                AppointmentDate = vDoctorSchedule.ScheduleDate,
                StartTime = vDoctorSchedule.StartTime,
                EndTime = vDoctorSchedule.EndTime,
                AppointmentType = d.AppointmentType,
                Reason = d.Reason,
                BookedDate = DateTime.UtcNow,
                CreatedBy = _currentUserService.UserId,
                ModifiedBy = _currentUserService.UserId
            };

            vDoctorSchedule.IsAvailable = false;

            await _db.Appointment.AddAsync(vAppointment);
            await _db.SaveChangesAsync();

            return ApiResponse<int>.SuccessResponse(vAppointment.AppointmentID, "Appointment Created Successfully.!");
        }
        #endregion


        #region CreateAppointmentByPatientAsync
        public async Task<ApiResponse<int>> CreateAppointmentByPatientAsync(PatientAppointmentAddDTO d)
        {
            var vDoctorSchedule = await _db.DoctorSchedules.FirstOrDefaultAsync(x => x.DoctorScheduleID == d.DoctorScheduleID && x.IsAvailable == true);

            if (vDoctorSchedule == null)
            {
                return ApiResponse<int>.ErrorResponse("Doctor Schedule Not Found.!");
            }

            var vAppointmentNo = await GenerateAppointmentNoAsync();

            int vUserID = _currentUserService.UserId;

            var vPatients = await _db.Patients.FirstOrDefaultAsync(x => x.UserID == vUserID);

            if (vPatients == null)
            {
                throw new Exception("Patient not found.");
            }

            int vPatientID = vPatients.PatientID;

            var vAppointment = new Appointment
            {
                AppointmentNo = vAppointmentNo,
                DoctorScheduleID = d.DoctorScheduleID,
                DoctorID = vDoctorSchedule.DoctorID,
                PatientID = vPatientID,
                AppointmentStatusID = 1,
                AppointmentDate = vDoctorSchedule.ScheduleDate,
                StartTime = vDoctorSchedule.StartTime,
                EndTime = vDoctorSchedule.EndTime,
                AppointmentType = d.AppointmentType,
                Reason = d.Reason,
                BookedDate = DateTime.UtcNow,
                CreatedBy = _currentUserService.UserId,
                ModifiedBy = _currentUserService.UserId
            };

            vDoctorSchedule.IsAvailable = false;

            await _db.Appointment.AddAsync(vAppointment);
            await _db.SaveChangesAsync();

            return ApiResponse<int>.SuccessResponse(vAppointment.AppointmentID, "Appointment Created Successfully.!");
        }
        #endregion


        #region GetLoggedInPatientAppointmentByIdAsync
        public async Task<ApiResponse<AppointmentResponseDTO>> GetLoggedInPatientAppointmentByIdAsync(int AppointmentID)
        {
            int vUserID = _currentUserService.UserId;
            int? vPatientID = await _db.Patients.Where(x => x.UserID == vUserID).Select(x => x.PatientID).FirstOrDefaultAsync();

            if (!vPatientID.HasValue)
                return ApiResponse<AppointmentResponseDTO>.ErrorResponse("Patient not found.");

            var appointment = await _db.Appointment
                .Where(x => x.AppointmentID == AppointmentID && x.PatientID == vPatientID)
                .Select(x => new AppointmentResponseDTO
                {
                    AppointmentID = x.AppointmentID,
                    AppointmentNo = x.AppointmentNo,
                    DoctorID = x.DoctorID,
                    DoctorName = x.Doctor.FirstName + " " + x.Doctor.LastName,
                    DoctorCode = x.Doctor.DoctorCode,
                    PatientID = x.PatientID,
                    PatientName = x.Patient.FirstName + " " + x.Patient.LastName,
                    PatientCode = x.Patient.PatientCode,
                    MobileNo = x.Patient.User.MobileNo,
                    AppointmentStatusID = x.AppointmentStatusID,
                    AppointmentStatusCode = x.AppointmentStatus.StatusCode,
                    AppointmentStatusName = x.AppointmentStatus.StatusName,
                    AppointmentType = x.AppointmentType,
                    AppointmentDate = x.AppointmentDate,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    BookedDate = x.BookedDate,
                    Reason = x.Reason,
                    PatientRemarks = x.PatientRemarks,
                    DoctorRemarks = x.DoctorRemarks,
                    ConfirmedDate = x.ConfirmedDate,
                    CheckedInDate = x.CheckedInDate,
                    StartedDate = x.StartedDate,
                    CompletedDate = x.CompletedDate,
                    CancelledDate = x.CancelledDate,
                    CancellationReason = x.CancellationReason
                }).FirstOrDefaultAsync();

            if (appointment == null)
                return ApiResponse<AppointmentResponseDTO>.ErrorResponse("Appointment not found.");

            return ApiResponse<AppointmentResponseDTO>.SuccessResponse(appointment, "");
        }
        #endregion


        #region GetLoggedInPatientAppointmentsAsync
        public async Task<ApiResponse<List<AppointmentResponseDTO>>> GetLoggedInPatientAppointmentsAsync(AppointmentSearchFilterDTO d)
        {
            int vUserID = _currentUserService.UserId;
            int? vPatientID = await _db.Patients.Where(x => x.UserID == vUserID).Select(x => x.PatientID).FirstOrDefaultAsync();

            if (!vPatientID.HasValue)
                return ApiResponse<List<AppointmentResponseDTO>>.ErrorResponse("Patient not found.");

            DateTime vNow = DateTime.Now;
            DateTime vToday = vNow.Date;
            TimeOnly vCurrentTime = TimeOnly.FromDateTime(vNow);

            var query = _db.Appointment.Where(x => x.PatientID == vPatientID.Value).AsQueryable();

            if (!string.IsNullOrWhiteSpace(d.ViewType))
            {
                if (d.ViewType.Equals("Upcoming", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(x => x.AppointmentDate > vToday || (x.AppointmentDate == vToday && x.StartTime >= vCurrentTime));
                }
                else if (d.ViewType.Equals("History", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(x => x.AppointmentStatus.StatusCode == "CMP"
                                            || x.AppointmentStatus.StatusCode == "CNL"
                                            || x.AppointmentStatus.StatusCode == "REJ"
                                            || x.AppointmentStatus.StatusCode == "NSH"
                                            || x.AppointmentDate < vToday
                                            || (x.AppointmentDate == vToday && x.StartTime < vCurrentTime));
                }
            }

            if (d.Date.HasValue)
                query = query.Where(x => x.AppointmentDate.Date == d.Date.Value.Date);

            if (d.AppointmentStatusID != 0)
                query = query.Where(x => x.AppointmentStatus.AppointmentStatusID == d.AppointmentStatusID);

            var appointments = await query
                .Select(x => new AppointmentResponseDTO
                {
                    AppointmentID = x.AppointmentID,
                    AppointmentNo = x.AppointmentNo,
                    DoctorID = x.DoctorID,
                    DoctorName = x.Doctor.FirstName + " " + x.Doctor.LastName,
                    DoctorCode = x.Doctor.DoctorCode,
                    PatientID = x.PatientID,
                    PatientName = x.Patient.FirstName + " " + x.Patient.LastName,
                    PatientCode = x.Patient.PatientCode,
                    MobileNo = x.Patient.User.MobileNo,
                    AppointmentStatusID = x.AppointmentStatusID,
                    AppointmentStatusCode = x.AppointmentStatus.StatusCode,
                    AppointmentStatusName = x.AppointmentStatus.StatusName,
                    AppointmentType = x.AppointmentType,
                    AppointmentDate = x.AppointmentDate,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    BookedDate = x.BookedDate,
                    Reason = x.Reason,
                    PatientRemarks = x.PatientRemarks,
                    DoctorRemarks = x.DoctorRemarks,
                    ConfirmedDate = x.ConfirmedDate,
                    CheckedInDate = x.CheckedInDate,
                    StartedDate = x.StartedDate,
                    CompletedDate = x.CompletedDate,
                    CancelledDate = x.CancelledDate,
                    CancellationReason = x.CancellationReason
                })
                .OrderByDescending(x => x.AppointmentDate)
                .ThenByDescending(x => x.StartTime)
                .ToListAsync();

            return ApiResponse<List<AppointmentResponseDTO>>.SuccessResponse(appointments, "");
        }
        #endregion


        #region GetLoggedInDoctorAppointmentsAsync
        public async Task<ApiResponse<List<AppointmentResponseDTO>>> GetLoggedInDoctorAppointmentsAsync(DoctorAppointmentSearchFilterDTO d)
        {
            int vUserID = _currentUserService.UserId;
            int? vDoctorID = await _db.Doctors.Where(x => x.UserID == vUserID).Select(x => x.DoctorID).FirstOrDefaultAsync();

            if (!vDoctorID.HasValue)
                return ApiResponse<List<AppointmentResponseDTO>>.ErrorResponse("Doctor not found.");

            DateTime vNow = DateTime.Now;
            DateTime vToday = vNow.Date;
            TimeOnly vCurrentTime = TimeOnly.FromDateTime(vNow);

            var query = _db.Appointment.Where(x => x.DoctorID == vDoctorID.Value).AsQueryable();

            if (!string.IsNullOrWhiteSpace(d.ViewType))
            {
                if (d.ViewType.Equals("Today", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(x => x.AppointmentDate == vToday);
                }
                else if (d.ViewType.Equals("Upcoming", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(x => x.AppointmentStatus.StatusCode != "CNL" && x.AppointmentStatus.StatusCode != "REJ" && x.AppointmentStatus.StatusCode != "NSH" && (x.AppointmentDate > vToday || (x.AppointmentDate == vToday && x.StartTime >= vCurrentTime)));
                }
                else if (d.ViewType.Equals("History", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(x => x.AppointmentStatus.StatusCode == "CMP" || x.AppointmentStatus.StatusCode == "CNL" || x.AppointmentStatus.StatusCode == "REJ" || x.AppointmentStatus.StatusCode == "NSH" || x.AppointmentDate < vToday || (x.AppointmentDate == vToday && x.StartTime < vCurrentTime));
                }
            }

            if (d.Date.HasValue)
                query = query.Where(x => x.AppointmentDate == d.Date.Value.Date);

            if (d.AppointmentStatusID != 0)
                query = query.Where(x => x.AppointmentStatusID == d.AppointmentStatusID);

            var appointments = await query
                .Select(x => new AppointmentResponseDTO
                {
                    AppointmentID = x.AppointmentID,
                    AppointmentNo = x.AppointmentNo,
                    DoctorID = x.DoctorID,
                    DoctorName = x.Doctor.FirstName + " " + x.Doctor.LastName,
                    DoctorCode = x.Doctor.DoctorCode,
                    PatientID = x.PatientID,
                    PatientName = x.Patient.FirstName + " " + x.Patient.LastName,
                    PatientCode = x.Patient.PatientCode,
                    MobileNo = x.Patient.User.MobileNo,
                    AppointmentStatusID = x.AppointmentStatusID,
                    AppointmentStatusCode = x.AppointmentStatus.StatusCode,
                    AppointmentStatusName = x.AppointmentStatus.StatusName,
                    AppointmentType = x.AppointmentType,
                    AppointmentDate = x.AppointmentDate,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    BookedDate = x.BookedDate,
                    Reason = x.Reason,
                    PatientRemarks = x.PatientRemarks,
                    DoctorRemarks = x.DoctorRemarks,
                    ConfirmedDate = x.ConfirmedDate,
                    CheckedInDate = x.CheckedInDate,
                    StartedDate = x.StartedDate,
                    CompletedDate = x.CompletedDate,
                    CancelledDate = x.CancelledDate,
                    CancellationReason = x.CancellationReason
                })
                .OrderBy(x => x.AppointmentDate)
                .ThenBy(x => x.StartTime)
                .ToListAsync();

            return ApiResponse<List<AppointmentResponseDTO>>.SuccessResponse(appointments, "");
        }
        #endregion


        #region GetLoggedInDoctorAppointmentByIdAsync
        public async Task<ApiResponse<AppointmentResponseDTO>> GetLoggedInDoctorAppointmentByIdAsync(int AppointmentID)
        {
            int vUserID = _currentUserService.UserId;
            int? vDoctorID = await _db.Doctors.Where(x => x.UserID == vUserID).Select(x => x.DoctorID).FirstOrDefaultAsync();

            if (!vDoctorID.HasValue)
                return ApiResponse<AppointmentResponseDTO>.ErrorResponse("Doctor not found.");

            var appointment = await _db.Appointment
                .Where(x => x.AppointmentID == AppointmentID && x.DoctorID == vDoctorID.Value)
                .Select(x => new AppointmentResponseDTO
                {
                    AppointmentID = x.AppointmentID,
                    AppointmentNo = x.AppointmentNo,
                    DoctorID = x.DoctorID,
                    DoctorName = x.Doctor.FirstName + " " + x.Doctor.LastName,
                    DoctorCode = x.Doctor.DoctorCode,
                    PatientID = x.PatientID,
                    PatientName = x.Patient.FirstName + " " + x.Patient.LastName,
                    PatientCode = x.Patient.PatientCode,
                    MobileNo = x.Patient.User.MobileNo,
                    AppointmentStatusID = x.AppointmentStatusID,
                    AppointmentStatusCode = x.AppointmentStatus.StatusCode,
                    AppointmentStatusName = x.AppointmentStatus.StatusName,
                    AppointmentType = x.AppointmentType,
                    AppointmentDate = x.AppointmentDate,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    BookedDate = x.BookedDate,
                    Reason = x.Reason,
                    PatientRemarks = x.PatientRemarks,
                    DoctorRemarks = x.DoctorRemarks,
                    ConfirmedDate = x.ConfirmedDate,
                    CheckedInDate = x.CheckedInDate,
                    StartedDate = x.StartedDate,
                    CompletedDate = x.CompletedDate,
                    CancelledDate = x.CancelledDate,
                    CancellationReason = x.CancellationReason
                })
                .FirstOrDefaultAsync();

            if (appointment == null)
                return ApiResponse<AppointmentResponseDTO>.ErrorResponse("Appointment not found.");

            return ApiResponse<AppointmentResponseDTO>.SuccessResponse(appointment, "");
        }
        #endregion


        #region CancelAppointmentByPatientAsync
        public async Task<ApiResponse<bool>> CancelAppointmentByPatientAsync(AppointmentCancelDTO d)
        {
            int vUserID = _currentUserService.UserId;

            var vPatient = await _db.Patients.FirstOrDefaultAsync(x => x.UserID == vUserID);

            if (vPatient == null)
                return ApiResponse<bool>.ErrorResponse("Patient not found.");

            var vAppointment = await _db.Appointment.FirstOrDefaultAsync(x => x.AppointmentID == d.AppointmentID && x.PatientID == vPatient.PatientID);

            if (vAppointment == null)
                return ApiResponse<bool>.ErrorResponse("Appointment not found.");

            if (vAppointment.CancelledDate.HasValue)
                return ApiResponse<bool>.ErrorResponse("Appointment is already cancelled.");

            vAppointment.AppointmentStatusID = await _db.AppointmentStatus.Where(x => x.StatusCode == "CNL").Select(x => x.AppointmentStatusID).FirstOrDefaultAsync();
            vAppointment.CancelledDate = DateTime.UtcNow;
            vAppointment.CancellationReason = d.CancellationReason;
            vAppointment.ModifiedBy = vUserID;

            await _db.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Appointment cancelled successfully!");
        }
        #endregion


        #region CancelAppointmentByAdminAsync
        public async Task<ApiResponse<bool>> CancelAppointmentByAdminAsync(AppointmentCancelDTO d)
        {
            var vAppointment = await _db.Appointment.FirstOrDefaultAsync(x => x.AppointmentID == d.AppointmentID);

            if (vAppointment == null)
                return ApiResponse<bool>.ErrorResponse("Appointment not found.");

            if (vAppointment.CancelledDate.HasValue)
                return ApiResponse<bool>.ErrorResponse("Appointment is already cancelled.");

            vAppointment.AppointmentStatusID = await _db.AppointmentStatus.Where(x => x.StatusCode == "CNL").Select(x => x.AppointmentStatusID).FirstOrDefaultAsync();
            vAppointment.CancelledDate = DateTime.UtcNow;
            vAppointment.CancellationReason = d.CancellationReason;
            vAppointment.ModifiedBy = _currentUserService.UserId;

            await _db.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Appointment cancelled successfully!");
        }
        #endregion


        #region UpdateAppointmentStatusAsync
        public async Task<ApiResponse<bool>> UpdateAppointmentStatusAsync(AppointmentStatusUpdateDTO d)
        {
            int vUserID = _currentUserService.UserId;

            var vAppointment = await _db.Appointment
                .FirstOrDefaultAsync(x => x.AppointmentID == d.AppointmentID);

            if (vAppointment == null)
                return ApiResponse<bool>.ErrorResponse("Appointment not found.");

            if (_currentUserService.IsDoctor == true)
            {
                var vDoctor = await _db.Doctors
                    .FirstOrDefaultAsync(x => x.UserID == vUserID);

                if (vDoctor == null)
                    return ApiResponse<bool>.ErrorResponse("Doctor not found.");

                if (vAppointment.DoctorID != vDoctor.DoctorID)
                    return ApiResponse<bool>.ErrorResponse("You are not authorized to update this appointment.");
            }
            else if (_currentUserService.IsAdmin != true)
            {
                return ApiResponse<bool>.ErrorResponse("You are not authorized to update appointment status.");
            }

            var vStatusExists = await _db.AppointmentStatus
                .AnyAsync(x => x.AppointmentStatusID == d.AppointmentStatusID);

            if (!vStatusExists)
                return ApiResponse<bool>.ErrorResponse("Invalid appointment status.");

            vAppointment.AppointmentStatusID = d.AppointmentStatusID;
            vAppointment.ModifiedBy = vUserID;

            await _db.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Appointment status updated successfully!");
        }
        #endregion


        #region GenerateAppointmentNoAsync
        private async Task<string> GenerateAppointmentNoAsync()
        {
            var year = DateTime.UtcNow.Year;

            var lastAppointmentNo = await _db.Appointment
                .Where(x => x.AppointmentNo.StartsWith($"APT-{year}-"))
                .OrderByDescending(x => x.AppointmentNo)
                .Select(x => x.AppointmentNo)
                .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (!string.IsNullOrEmpty(lastAppointmentNo))
            {
                var lastNumber = lastAppointmentNo.Split('-').Last();

                nextNumber = int.Parse(lastNumber) + 1;
            }

            return $"APT-{year}-{nextNumber:D6}";
        }
        #endregion
    }
}
