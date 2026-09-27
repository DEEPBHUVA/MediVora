using MediVora.Common;
using MediVora.Data.EF;
using MediVora.DTO;
using MediVora.Entities;
using MediVora.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MediVora.Services
{
    public class DoctorService : IDoctorService
    {
        public readonly AppDbContext _db;
        public readonly ICurrentUserService _currentUser;
        private readonly ICacheService _cacheService;

        public DoctorService(AppDbContext db, ICurrentUserService currentUser, ICacheService cacheService)
        {
            _db = db;
            _currentUser = currentUser;
            _cacheService = cacheService;
        }

        #region CreateDoctorAsync
        public async Task<ApiResponse<DoctorResponseDTO>> CreateDoctorAsync(DoctorAddDTO doctorRequestDTO)
        {
            // Check email
            var userExists = await _db.Users.AnyAsync(x => x.Email == doctorRequestDTO.Email);

            if (userExists)
            {
                return ApiResponse<DoctorResponseDTO>.ErrorResponse("User with this email already exists.");
            }

            // Check doctor code
            var doctorExists = await _db.Doctors.AnyAsync(x => x.DoctorCode == doctorRequestDTO.DoctorCode);

            if (doctorExists)
            {
                return ApiResponse<DoctorResponseDTO>.ErrorResponse("Doctor with this code already exists.");
            }

            var newUser = new User
            {
                FirstName = doctorRequestDTO.FirstName,
                LastName = doctorRequestDTO.LastName,
                Email = doctorRequestDTO.Email,
                MobileNo = doctorRequestDTO.MobileNo,
                RoleID = 3,
                UserName = doctorRequestDTO.UserName,
                PasswordHash = PasswordHelper.HashPassword(doctorRequestDTO.Password),
                IsActive = doctorRequestDTO.IsActive,
                LastLoginDate = null
            };

            await _db.Users.AddAsync(newUser);
            await _db.SaveChangesAsync();

            var newDoctor = new Doctors
            {
                UserID = newUser.UserID,
                DepartmentID = doctorRequestDTO.DepartmentID,
                DoctorCode = doctorRequestDTO.DoctorCode,
                FirstName = doctorRequestDTO.FirstName,
                MiddleName = doctorRequestDTO.MiddleName,
                LastName = doctorRequestDTO.LastName,
                Qualification = doctorRequestDTO.Qualification,
                Specialization = doctorRequestDTO.Specialization,
                MedicalRegistrationNo = doctorRequestDTO.MedicalRegistrationNo,
                ConsultationFee = doctorRequestDTO.ConsultationFee,
                ProfileImagePath = doctorRequestDTO.ProfileImagePath,
                IsActive = doctorRequestDTO.IsActive,
                CreatedBy = _currentUser.UserId,
                ModifiedBy = _currentUser.UserId
            };

            await _db.Doctors.AddAsync(newDoctor);
            await _db.SaveChangesAsync();

            var response = new DoctorResponseDTO
            {
                DoctorID = newDoctor.DoctorID,
                DoctorCode = newDoctor.DoctorCode,
                FirstName = newDoctor.FirstName,
                MiddleName = newDoctor.MiddleName,
                LastName = newDoctor.LastName,
                Qualification = newDoctor.Qualification,
                Specialization = newDoctor.Specialization,
                MedicalRegistrationNo = newDoctor.MedicalRegistrationNo,
                ConsultationFee = newDoctor.ConsultationFee,
                ProfileImagePath = newDoctor.ProfileImagePath,
                IsActive = newDoctor.IsActive,
                CreatedBy = _currentUser.UserId,
                ModifiedBy = _currentUser.UserId,
                Created = newDoctor.Created,
                Modified = newDoctor.Modified
            };

            return ApiResponse<DoctorResponseDTO>.SuccessResponse(response, "Doctor created successfully.");
        }
        #endregion


        #region DeleteDoctorAsync
        public async Task<ApiResponse<bool>> DeleteDoctorAsync(int id)
        {
            var vExistingDoctor = await _db.Doctors.FindAsync(id);
            if (vExistingDoctor == null)
            {
                return ApiResponse<bool>.ErrorResponse("Doctor not found.");
            }

            var vExistingUser = await _db.Users.FindAsync(vExistingDoctor.UserID);

            vExistingUser.IsActive = false;
            vExistingDoctor.IsActive = false;
            vExistingDoctor.IsTrashed = true;
            await _db.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Doctor deleted successfully.");
        }
        #endregion


        #region GetAllDoctorsAsync
        public async Task<ApiResponse<List<DoctorResponseDTO>>> GetAllDoctorsAsync()
        {
            var vDoctorList = await _db.Doctors.AsNoTracking()
                .Where(d => d.IsActive && !d.IsTrashed)
                .Select(d => new DoctorResponseDTO
                {
                    DoctorID = d.DoctorID,
                    DepartmentID = d.DepartmentID,
                    DepartmentName = d.Department.DepartmentName,
                    DoctorCode = d.DoctorCode,
                    FirstName = d.FirstName,
                    MiddleName = d.MiddleName,
                    LastName = d.LastName,
                    Qualification = d.Qualification,
                    Specialization = d.Specialization,
                    MedicalRegistrationNo = d.MedicalRegistrationNo,
                    ConsultationFee = d.ConsultationFee,
                    ProfileImagePath = d.ProfileImagePath,
                    IsActive = d.IsActive,
                    CreatedBy = d.CreatedBy,
                    ModifiedBy = d.ModifiedBy,
                    Created = d.Created,
                    Modified = d.Modified
                })
                .ToListAsync();

            return ApiResponse<List<DoctorResponseDTO>>.SuccessResponse(vDoctorList, "Doctors retrieved successfully.");
        }
        #endregion


        #region GetDoctorByIdAsync
        public async Task<ApiResponse<DoctorResponseDTO>> GetDoctorByIdAsync(int id)
        {
            var vDoctor = await _db.Doctors.AsNoTracking()
                .Where(d => d.DoctorID == id && d.IsActive && !d.IsTrashed)
                .Select(d => new DoctorResponseDTO
                {
                    DoctorID = d.DoctorID,
                    DepartmentID = d.DepartmentID,
                    DepartmentName = d.Department.DepartmentName,
                    DoctorCode = d.DoctorCode,
                    FirstName = d.FirstName,
                    MiddleName = d.MiddleName,
                    LastName = d.LastName,
                    Qualification = d.Qualification,
                    Specialization = d.Specialization,
                    MedicalRegistrationNo = d.MedicalRegistrationNo,
                    ConsultationFee = d.ConsultationFee,
                    ProfileImagePath = d.ProfileImagePath,
                    IsActive = d.IsActive,
                    CreatedBy = d.CreatedBy,
                    ModifiedBy = d.ModifiedBy,
                    Created = d.Created,
                    Modified = d.Modified
                })
                .FirstOrDefaultAsync();

            if (vDoctor == null)
            {
                return ApiResponse<DoctorResponseDTO>.ErrorResponse("Doctor not found.");
            }

            return ApiResponse<DoctorResponseDTO>.SuccessResponse(vDoctor, "Doctor retrieved successfully.");
        }
        #endregion


        #region GetDoctorSearchByFilterAsync
        public async Task<ApiResponse<List<DoctorResponseDTO>>> GetDoctorSearchByFilterAsync(DoctorSearchFilterDTO doctorSearchFilterDTO)
        {
            var cacheKey = GenerateDoctorSearchCacheKey(doctorSearchFilterDTO);

            var cachedDoctors = await _cacheService.GetDataAsync<List<DoctorResponseDTO>>(cacheKey);

            if (cachedDoctors is not null)
            {
                return ApiResponse<List<DoctorResponseDTO>>.SuccessResponse(cachedDoctors, "Doctors retrieved successfully.");
            }

            var query = _db.Doctors.AsNoTracking().Where(d => d.IsActive && !d.IsTrashed);

            if (doctorSearchFilterDTO.DepartmentID.HasValue)
            {
                query = query.Where(d => d.DepartmentID == doctorSearchFilterDTO.DepartmentID.Value);
            }

            if (!string.IsNullOrWhiteSpace(doctorSearchFilterDTO.DoctorName))
            {
                var doctorName = doctorSearchFilterDTO.DoctorName.Trim();

                query = query.Where(d => (d.FirstName + " " + d.MiddleName + " " + d.LastName).Contains(doctorName));
            }

            if (!string.IsNullOrWhiteSpace(doctorSearchFilterDTO.Specialization))
            {
                var specialization = doctorSearchFilterDTO.Specialization.Trim();

                query = query.Where(d => d.Specialization.Contains(specialization));
            }

            if (!string.IsNullOrWhiteSpace(doctorSearchFilterDTO.Qualification))
            {
                var qualification = doctorSearchFilterDTO.Qualification.Trim();

                query = query.Where(d => d.Qualification.Contains(qualification));
            }

            if (doctorSearchFilterDTO.MinConsultationFee.HasValue)
            {
                query = query.Where(d =>
                    d.ConsultationFee >= doctorSearchFilterDTO.MinConsultationFee.Value);
            }

            if (doctorSearchFilterDTO.MaxConsultationFee.HasValue)
            {
                query = query.Where(d => d.ConsultationFee <= doctorSearchFilterDTO.MaxConsultationFee.Value);
            }

            var vDoctorList = await query
                .Select(d => new DoctorResponseDTO
                {
                    DoctorID = d.DoctorID,
                    DepartmentID = d.DepartmentID,
                    DepartmentName = d.Department.DepartmentName,
                    DoctorCode = d.DoctorCode,
                    FirstName = d.FirstName,
                    MiddleName = d.MiddleName,
                    LastName = d.LastName,
                    Qualification = d.Qualification,
                    Specialization = d.Specialization,
                    MedicalRegistrationNo = d.MedicalRegistrationNo,
                    ConsultationFee = d.ConsultationFee,
                    ProfileImagePath = d.ProfileImagePath,
                    IsActive = d.IsActive,
                    CreatedBy = d.CreatedBy,
                    ModifiedBy = d.ModifiedBy,
                    Created = d.Created,
                    Modified = d.Modified
                })
                .ToListAsync();


            if (vDoctorList.Count > 0)
            {
                await _cacheService.SetDataAsync(cacheKey, vDoctorList, TimeSpan.FromMinutes(2));
            }

            return ApiResponse<List<DoctorResponseDTO>>.SuccessResponse(vDoctorList, "Doctors retrieved successfully.");
        }

        private string GenerateDoctorSearchCacheKey(DoctorSearchFilterDTO filter)
        {
            var rawKey = string.Join("|",
                filter.DepartmentID?.ToString() ?? "",
                filter.DoctorName?.Trim().ToLowerInvariant() ?? "",
                filter.Specialization?.Trim().ToLowerInvariant() ?? "",
                filter.Qualification?.Trim().ToLowerInvariant() ?? "",
                filter.MinConsultationFee?.ToString() ?? "",
                filter.MaxConsultationFee?.ToString() ?? ""
            );

            var hash = SHA256.HashData(
                Encoding.UTF8.GetBytes(rawKey));

            var hashString = Convert.ToHexString(hash);

            return $"doctor-search:{hashString}";
        }
        #endregion


        #region UpdateDoctorAsync
        public async Task<ApiResponse<DoctorResponseDTO>> UpdateDoctorAsync(int id, DoctorEditDTO doctorRequestDTO)
        {
            var vExistingDoctor = await _db.Doctors.FindAsync(id);

            if (vExistingDoctor == null)
            {
                return ApiResponse<DoctorResponseDTO>.ErrorResponse("Doctor not found.");
            }

            vExistingDoctor.DepartmentID = doctorRequestDTO.DepartmentID;
            vExistingDoctor.DoctorCode = doctorRequestDTO.DoctorCode;
            vExistingDoctor.FirstName = doctorRequestDTO.FirstName;
            vExistingDoctor.MiddleName = doctorRequestDTO.MiddleName;
            vExistingDoctor.LastName = doctorRequestDTO.LastName;
            vExistingDoctor.Qualification = doctorRequestDTO.Qualification;
            vExistingDoctor.Specialization = doctorRequestDTO.Specialization;
            vExistingDoctor.MedicalRegistrationNo = doctorRequestDTO.MedicalRegistrationNo;
            vExistingDoctor.ConsultationFee = doctorRequestDTO.ConsultationFee;
            vExistingDoctor.IsActive = doctorRequestDTO.IsActive;
            vExistingDoctor.ModifiedBy = _currentUser.UserId;

            vExistingDoctor.ProfileImagePath = doctorRequestDTO.ProfileImagePath ?? vExistingDoctor.ProfileImagePath;

            if (!string.IsNullOrEmpty(doctorRequestDTO.ProfileImagePath))
            {
                vExistingDoctor.ProfileImagePath = doctorRequestDTO.ProfileImagePath;
            }

            await _db.SaveChangesAsync();

            return ApiResponse<DoctorResponseDTO>.SuccessResponse(
                new DoctorResponseDTO
                {
                    DoctorID = vExistingDoctor.DoctorID,
                    DepartmentID = vExistingDoctor.DepartmentID,
                    // DepartmentName = vExistingDoctor.Department?.DepartmentName,
                    DoctorCode = vExistingDoctor.DoctorCode,
                    FirstName = vExistingDoctor.FirstName,
                    MiddleName = vExistingDoctor.MiddleName,
                    LastName = vExistingDoctor.LastName,
                    Qualification = vExistingDoctor.Qualification,
                    Specialization = vExistingDoctor.Specialization,
                    MedicalRegistrationNo = vExistingDoctor.MedicalRegistrationNo,
                    ConsultationFee = vExistingDoctor.ConsultationFee,
                    ProfileImagePath = vExistingDoctor.ProfileImagePath,
                    IsActive = vExistingDoctor.IsActive,
                    CreatedBy = vExistingDoctor.CreatedBy,
                    ModifiedBy = vExistingDoctor.ModifiedBy,
                    Created = vExistingDoctor.Created,
                    Modified = vExistingDoctor.Modified
                }, "Doctor updated successfully.");
        }
        #endregion
    }
}
