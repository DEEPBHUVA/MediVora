using MediVora.Common;
using MediVora.Data.EF;
using MediVora.DTO;
using MediVora.Entities;
using MediVora.Interfaces;
using MediVora.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MediVora.Services
{
    public class AuthService : IAuthService
    {
        public readonly AppDbContext _db;
        private readonly JWT _jwt;
        public AuthService(AppDbContext db, JWT jwt)
        {
            _db = db;
            _jwt = jwt;
        }


        #region LoginAsync
        public async Task<LoginResponse> LoginAsync(LoginDTO loginDTO)
        {
            var vLoginResponse = new LoginResponse();

            var vUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (vUser == null)
            {
                vLoginResponse.Message = "User not found.";
                vLoginResponse.IsAuthenticated = false;

                return vLoginResponse;
            }

            bool vIsPasswordValid = PasswordHelper.VerifyPassword(loginDTO.Password, vUser.PasswordHash);
            if (!vIsPasswordValid)
            {
                vLoginResponse.Message = $"Incorrect password for user with email {loginDTO.Email}";
                vLoginResponse.IsAuthenticated = false;
            }

            vUser.LastLoginDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            var vJwtSecurityToken = await GenerateToken(vUser);

            return new LoginResponse
            {
                Message = "Login successful.",
                IsAuthenticated = true,
                UserName = vUser.UserName,
                Email = vUser.Email,
                RoleID = vUser.RoleID,
                Token = new JwtSecurityTokenHandler().WriteToken(vJwtSecurityToken)
            };
        }
        #endregion


        #region RegisterAsync
        public async Task<RegisterResponse> RegisterAsync(RegisterDTO registerDTO)
        {
            var vRegisterResponse = new RegisterResponse();

            var vExistingUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == registerDTO.Email);

            if (vExistingUser != null)
            {
                vRegisterResponse.Message = "User already exists.";
                vRegisterResponse.IsAuthenticated = false;
                return vRegisterResponse;
            }

            var vUser = new User
            {
                UserName = registerDTO.Username,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                RoleID = registerDTO.RoleID, // Assuming 4 is the default role ID for a patient or regular user. Adjust as necessary.
                Email = registerDTO.Email,
                MobileNo = registerDTO.MobileNo,
                PasswordHash = PasswordHelper.HashPassword(registerDTO.Password),
                IsActive = true,
                LastLoginDate = DateTime.UtcNow,
                //Created = DateTime.UtcNow,
                //Modified = DateTime.UtcNow,
            };

            await _db.Users.AddAsync(vUser);
            await _db.SaveChangesAsync();


            #region Insert Record Into Patient Table
            if (vUser.RoleID == 4)
            {
                var vPatient = new Patients
                {
                    UserID = vUser.UserID,
                    PatientCode = await GeneratePatientCodeAsync(),
                    FirstName = vUser.FirstName,
                    LastName = vUser.LastName,
                    IsActive = true,
                };

                await _db.Patients.AddAsync(vPatient);
                await _db.SaveChangesAsync();
            }
            #endregion


            return new RegisterResponse
            {
                Message = "User registered successfully.",
                IsAuthenticated = true,
                UserName = vUser.UserName,
                Email = vUser.Email,
                RoleID = vUser.RoleID,
                Token = new JwtSecurityTokenHandler().WriteToken(await GenerateToken(vUser))
            };
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


        #region GetLoggedInUserAsync
        public async Task<ApiResponse<UserResponseDTO>> GetLoggedInUserAsync(string email)
        {
            var user = await _db.Users.Include(x => x.Roles)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return ApiResponse<UserResponseDTO>.ErrorResponse("User not found.");
            }

            var vUserResponse = new UserResponseDTO
            {
                UserID = user.UserID,
                RoleID = user.RoleID,
                RoleName = user.Roles.RoleName,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                MobileNo = user.MobileNo,
                IsActive = user.IsActive,
                LastLoginDate = user.LastLoginDate
            };

            return ApiResponse<UserResponseDTO>.SuccessResponse(
                vUserResponse,
                "User retrieved successfully."
            );
        }
        #endregion


        #region GenerateToken
        private async Task<JwtSecurityToken> GenerateToken(User vUser)
        {
            var vRole = await _db.Roles.FirstOrDefaultAsync(r => r.RoleID == vUser.RoleID);

            var vClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, vUser.UserID.ToString()),
                new Claim(ClaimTypes.Name, vUser.UserName),
                new Claim(ClaimTypes.Email, vUser.Email),
                new Claim(ClaimTypes.Role, vRole.RoleName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var vSymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var vSigningCredentials = new SigningCredentials(vSymmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var vJwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: vClaims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: vSigningCredentials);

            return vJwtSecurityToken;
        }
        #endregion

    }
}
