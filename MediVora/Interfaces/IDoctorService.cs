using MediVora.Common;
using MediVora.DTO;

namespace MediVora.Interfaces
{
    public interface IDoctorService
    {
        public Task<ApiResponse<List<DoctorResponseDTO>>> GetAllDoctorsAsync();
        public Task<ApiResponse<DoctorResponseDTO>> GetDoctorByIdAsync(int id);
        public Task<ApiResponse<DoctorResponseDTO>> CreateDoctorAsync(DoctorAddDTO doctorRequestDTO);
        public Task<ApiResponse<DoctorResponseDTO>> UpdateDoctorAsync(int id, DoctorEditDTO doctorRequestDTO);
        public Task<ApiResponse<bool>> DeleteDoctorAsync(int id);
        public Task<ApiResponse<List<DoctorResponseDTO>>> GetDoctorSearchByFilterAsync(DoctorSearchFilterDTO doctorSearchFilterDTO);
    }
}
