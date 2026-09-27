using MediVora.Common;
using MediVora.DTO;

namespace MediVora.Interfaces
{
    public interface IDoctorScheduleService
    {
        public Task<ApiResponse<int>> CreateDoctorScheduleAsync(int DoctorID, DoctorScheduleAddEditDTO d);
        public Task<ApiResponse<bool>> UpdateDoctorScheduleAsync(int DoctorID, int id, DoctorScheduleAddEditDTO d);
        public Task<ApiResponse<List<DoctorScheduleResponseDTO>>> GetDoctorSchedulesAsync(int? DoctorID = null);
        public Task<ApiResponse<DoctorScheduleResponseDTO>> GetDoctorScheduleByIdAsync(int DoctorID, int id);
        public Task<ApiResponse<bool>> DeleteDoctorScheduleAsync(int DoctorID, int id);
        public Task<ApiResponse<bool>> UpdateDoctorScheduleStatusAsync(int DoctorID, int id, bool IsAvailable);

        // For generating doctor schedule based on the given date range
        public Task<ApiResponse<bool>> GenerateDoctorScheduleAsync(int DoctorID, GenerateDoctorScheduleDTO d);
        public Task<ApiResponse<List<DoctorScheduleResponseDTO>>> GetDoctorSchedulesByDateRangeAsync(DateTime startDate, DateTime endDate, int? DoctorID = null);
    }
}
