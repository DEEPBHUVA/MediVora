using MediVora.Common;
using MediVora.DTO;

namespace MediVora.Interfaces
{
    public interface IDoctorScheduleTemplateService
    {
        public Task<ApiResponse<int>> CreateDoctorScheduleTemplateAsync(int DoctorID, DoctorScheduleTemplateAddEditDTO d);
        public Task<ApiResponse<bool>> UpdateDoctorScheduleTemplateAsync(int DoctorID, int id, DoctorScheduleTemplateAddEditDTO d);
        public Task<ApiResponse<DoctorScheduleTemplateResponseDTO>> GetDoctorScheduleTemplateByIdAsync(int DoctorID, int id);
        public Task<ApiResponse<List<DoctorScheduleTemplateResponseDTO>>> GetAllDoctorScheduleTemplatesAsync(int DoctorID);
        public Task<ApiResponse<bool>> DeleteDoctorScheduleTemplateAsync(int DoctorID, int id);
        public Task<ApiResponse<bool>> UpdateDoctorScheduleTemplateStatusAsync(int DoctorID, int id, bool IsActive);
    }
}
