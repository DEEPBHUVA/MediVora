using MediVora.Common;
using MediVora.Data.EF;
using MediVora.DTO;
using MediVora.Entities;
using MediVora.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;

namespace MediVora.Services
{
    public class PatientService : IPatientService
    {
        public readonly AppDbContext _db;
        public readonly ICurrentUserService _currentUserService;

        public PatientService(AppDbContext db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        #region CreatePatientProfileAsync
        public async Task<ApiResponse<int>> CreatePatientProfileAsync(PatientAddDTO d)
        {
            var vExistingPatient = await _db.Patients.AnyAsync(x => x.User.Email == d.Email);

            if (vExistingPatient)
            {
                return ApiResponse<int>.ErrorResponse("Patient already exists with this email address.");
            }

            var vUser = new User
            {
                RoleID = d.RoleID,
                UserName = d.UserName,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email,
                MobileNo = d.MobileNo,
                PasswordHash = PasswordHelper.HashPassword(d.Password),
                IsActive = d.IsActive,
            };

            var vPatient = new Patients
            {
                User = vUser,
                PatientCode = await GeneratePatientCodeAsync(),
                FirstName = d.FirstName,
                MiddleName = d.MiddleName,
                LastName = d.LastName,
                DateOfBirth = d.DateOfBirth,
                Gender = d.Gender,
                BloodGroup = d.BloodGroup,
                Address = d.Address,
                City = d.City,
                State = d.State,
                PostalCode = d.PostalCode,
                EmergencyContactName = d.EmergencyContactName,
                EmergencyContactNumber = d.EmergencyContactNumber,
                IsActive = d.IsActive
            };

            await _db.Patients.AddAsync(vPatient);
            await _db.SaveChangesAsync();

            return ApiResponse<int>.SuccessResponse(vPatient.PatientID, "Record saved successfully!");
        }

        #region GeneratePatientCodeAsync
        private async Task<string> GeneratePatientCodeAsync()
        {
            int vYear = DateTime.UtcNow.Year;

            var vLastCode = await _db.Patients.Where(p => p.PatientCode.StartsWith($"PAT-{vYear}-"))
                .OrderByDescending(p => p.PatientCode)
                .Select(p => p.PatientCode)
                .FirstOrDefaultAsync();

            int vNextNumber = 1;

            if (!string.IsNullOrEmpty(vLastCode))
            {
                vNextNumber = int.Parse(vLastCode.Split('-')[2]) + 1;
            }

            return $"PAT-{vYear}-{vNextNumber:D6}";
        }
        #endregion
        #endregion


        #region GetPatientByIdAsync
        public async Task<ApiResponse<PatientResponseDTO>> GetPatientByIdAsync(int PatientID)
        {
            if (PatientID <= 0)
            {
                return ApiResponse<PatientResponseDTO>.ErrorResponse("Invalid patient ID");
            }

            var vPatient = await _db.Patients.AsNoTracking().Include(p => p.User)
                .Where(p => p.PatientID == PatientID && p.IsActive)
                .Select(p => new PatientResponseDTO
                {
                    PatientID = p.PatientID,
                    UserID = p.UserID,
                    PatientCode = p.PatientCode,
                    FirstName = p.FirstName,
                    MiddleName = p.MiddleName,
                    LastName = p.LastName,
                    Email = p.User.Email,
                    MobileNo = p.User.MobileNo,
                    DateOfBirth = p.DateOfBirth,
                    Gender = p.Gender,
                    BloodGroup = p.BloodGroup,
                    Address = p.Address,
                    City = p.City,
                    State = p.State,
                    PostalCode = p.PostalCode,
                    EmergencyContactName = p.EmergencyContactName,
                    EmergencyContactNumber = p.EmergencyContactNumber,
                    IsActive = p.IsActive,
                    Created = p.Created,
                    Modified = p.Modified
                }).FirstOrDefaultAsync();

            if (vPatient == null)
            {
                return ApiResponse<PatientResponseDTO>.ErrorResponse("Patient not found");
            }

            return ApiResponse<PatientResponseDTO>.SuccessResponse(vPatient, "Patient retrieved successfully.");
        }
        #endregion


        #region GetPatientsAsync
        public async Task<ApiResponse<List<PatientResponseDTO>>> GetPatientsAsync()
        {
            var vPatients = await _db.Patients.Include(x => x.User).AsNoTracking()
                .Select(x => new PatientResponseDTO
                {
                    PatientID = x.PatientID,
                    UserID = x.UserID,
                    PatientCode = x.PatientCode,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    Email = x.User.Email,
                    MobileNo = x.User.MobileNo,
                    DateOfBirth = x.DateOfBirth,
                    Gender = x.Gender,
                    BloodGroup = x.BloodGroup,
                    Address = x.Address,
                    City = x.City,
                    State = x.State,
                    PostalCode = x.PostalCode,
                    EmergencyContactName = x.EmergencyContactName,
                    EmergencyContactNumber = x.EmergencyContactNumber,
                    IsActive = x.IsActive,
                    Created = x.Created,
                    Modified = x.Modified
                }).ToListAsync();

            return ApiResponse<List<PatientResponseDTO>>.SuccessResponse(vPatients, "Patients retrived successfully!");
        }
        #endregion


        #region UpdatePatientAsync
        public async Task<ApiResponse<bool>> UpdatePatientProfileAsync(int? PatientID, PatientEditDTO dto)
        {
            Patients? vPatient;

            // Admin
            if (_currentUserService.IsAdmin == true)
            {
                if (!PatientID.HasValue || PatientID.Value <= 0)
                {
                    return ApiResponse<bool>.ErrorResponse("Patient ID is required for admin.");
                }

                vPatient = await _db.Patients.Include(p => p.User).FirstOrDefaultAsync(p => p.PatientID == PatientID.Value);
            }
            else if (_currentUserService.IsPatient == true)
            {
                if (_currentUserService.UserId <= 0)
                {
                    return ApiResponse<bool>.ErrorResponse("Invalid logged-in user.");
                }

                vPatient = await _db.Patients
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p =>
                        p.UserID == _currentUserService.UserId &&
                        p.IsActive);
            }
            else
            {
                return ApiResponse<bool>.ErrorResponse("You are not authorized to update patient information.");
            }

            if (vPatient == null)
            {
                return ApiResponse<bool>.ErrorResponse("Patient not found.");
            }

            // Update patient fields
            vPatient.FirstName = dto.FirstName;
            vPatient.MiddleName = dto.MiddleName;
            vPatient.LastName = dto.LastName;
            vPatient.DateOfBirth = dto.DateOfBirth;
            vPatient.Gender = dto.Gender;
            vPatient.BloodGroup = dto.BloodGroup;
            vPatient.Address = dto.Address;
            vPatient.City = dto.City;
            vPatient.State = dto.State;
            vPatient.PostalCode = dto.PostalCode;
            vPatient.EmergencyContactName = dto.EmergencyContactName;
            vPatient.EmergencyContactNumber = dto.EmergencyContactNumber;

            var vUser = await _db.Users.FirstOrDefaultAsync(x => x.UserID == vPatient.UserID);
            if (vUser != null)
            {
                vUser.FirstName = dto.FirstName;
                vUser.LastName = dto.LastName;
                vUser.Email = dto.Email;
                vUser.MobileNo = dto.MobileNo;
            }

            await _db.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Patient updated successfully.");
        }
        #endregion


        #region UpdatePatientStatusAsync
        public async Task<ApiResponse<bool>> UpdatePatientStatusAsync(int PatientID, bool IsActive)
        {
            var vPatient = await _db.Patients.Include(x => x.User).FirstOrDefaultAsync(x => x.PatientID == PatientID);

            if (vPatient == null)
            {
                return ApiResponse<bool>.ErrorResponse("Patient not found.");
            }

            vPatient.IsActive = IsActive;
            vPatient.User.IsActive = IsActive;

            await _db.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Record updated successfully!");
        }
        #endregion



        #region GetCurrentPatientAsync
        public async Task<ApiResponse<PatientResponseDTO>> GetCurrentPatientAsync()
        {
            if (_currentUserService.IsPatient == false)
            {
                return ApiResponse<PatientResponseDTO>.ErrorResponse("You are not authorized to view patient information.");
            }

            var userId = _currentUserService.UserId;

            if (userId <= 0)
            {
                return ApiResponse<PatientResponseDTO>.ErrorResponse("Invalid logged-in user.");
            }

            var vPatient = await _db.Patients.AsNoTracking().Where(p => p.UserID == userId && p.IsActive)
                .Select(p => new PatientResponseDTO
                {
                    PatientID = p.PatientID,
                    UserID = p.UserID,
                    PatientCode = p.PatientCode,
                    FirstName = p.FirstName,
                    MiddleName = p.MiddleName,
                    LastName = p.LastName,
                    Email = p.User.Email,
                    MobileNo = p.User.MobileNo,
                    DateOfBirth = p.DateOfBirth,
                    Gender = p.Gender,
                    BloodGroup = p.BloodGroup,
                    Address = p.Address,
                    City = p.City,
                    State = p.State,
                    PostalCode = p.PostalCode,
                    EmergencyContactName = p.EmergencyContactName,
                    EmergencyContactNumber = p.EmergencyContactNumber,
                    IsActive = p.IsActive,
                    Created = p.Created,
                    Modified = p.Modified
                }).FirstOrDefaultAsync();

            if (vPatient == null)
            {
                return ApiResponse<PatientResponseDTO>
                    .ErrorResponse("Patient profile not found.");
            }

            return ApiResponse<PatientResponseDTO>.SuccessResponse(vPatient, "Patient profile retrieved successfully.");
        }
        #endregion
    }
}
