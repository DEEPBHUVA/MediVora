using MediVora.Common;
using MediVora.DTO;

namespace MediVora.Interfaces
{
    public interface IAppointmentService
    {
        public Task<ApiResponse<int>> CreateAppointmentByPatientAsync(PatientAppointmentAddDTO d);
        public Task<ApiResponse<int>> CreateAppointmentByAdminAsync(AdminAppointmentAddDTO d);
        public Task<ApiResponse<List<AppointmentResponseDTO>>> GetLoggedInPatientAppointmentsAsync(AppointmentSearchFilterDTO d);
        public Task<ApiResponse<AppointmentResponseDTO>> GetLoggedInPatientAppointmentByIdAsync(int AppointmentID);

        public Task<ApiResponse<bool>> CancelAppointmentByPatientAsync(AppointmentCancelDTO d);
        public Task<ApiResponse<bool>> CancelAppointmentByAdminAsync(AppointmentCancelDTO d);

        public Task<ApiResponse<List<AppointmentResponseDTO>>> GetLoggedInDoctorAppointmentsAsync(DoctorAppointmentSearchFilterDTO d);
        public Task<ApiResponse<AppointmentResponseDTO>> GetLoggedInDoctorAppointmentByIdAsync(int AppointmentID);

        public Task<ApiResponse<bool>> UpdateAppointmentStatusAsync(AppointmentStatusUpdateDTO dto);
    }
}
