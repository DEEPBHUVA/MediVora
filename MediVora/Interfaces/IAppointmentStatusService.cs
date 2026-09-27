using MediVora.Common;
using MediVora.DTO;

namespace MediVora.Interfaces
{
    public interface IAppointmentStatusService
    {
        public Task<ApiResponse<int>> CreateAppointmentStatusAsync(AppointmentStatusAddEditDTO d);
        public Task<ApiResponse<bool>> UpdateAppointmentStatusAsync(int AppointmentStatusID, AppointmentStatusAddEditDTO d);
        public Task<ApiResponse<bool>> DeleteAppointmentStatusAsync(int AppointmentStatusID);
        public Task<ApiResponse<AppointmentStatusResponseDTO>> GetAppointmentStatusByIdAsync(int AppointmentStatusID);
        public Task<ApiResponse<List<AppointmentStatusResponseDTO>>> GetAllAppointmentStatusAsync();
        public Task<ApiResponse<List<AppointmentStatusDropdownDTO>>> GetAppointmentStatusDropdownAsync();
    }
}
