using MediVora.Common;
using MediVora.DTO;

namespace MediVora.Interfaces
{
    public interface IPatientService
    {
        public Task<ApiResponse<List<PatientResponseDTO>>> GetPatientsAsync();
        public Task<ApiResponse<PatientResponseDTO>> GetPatientByIdAsync(int PatientID);
        public Task<ApiResponse<int>> CreatePatientProfileAsync(PatientAddDTO d);
        public Task<ApiResponse<bool>> UpdatePatientProfileAsync(int? PatientID, PatientEditDTO d);
        public Task<ApiResponse<bool>> UpdatePatientStatusAsync(int PatientID, bool IsActive);

        public Task<ApiResponse<PatientResponseDTO>> GetCurrentPatientAsync();

    }
}
