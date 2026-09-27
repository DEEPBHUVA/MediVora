using MediVora.Common;
using MediVora.DTO;

namespace MediVora.Interfaces
{
    public interface IDepartmentService
    {
        public Task<ApiResponse<List<DepartmentResponseDTO>>> GetAllDepartmentsAsync();
        public Task<ApiResponse<DepartmentResponseDTO>> GetDepartmentByIdAsync(int departmentId);
        public Task<ApiResponse<DepartmentResponseDTO>> CreateDepartmentAsync(DepartmentAddEditDTO departmentRequestDTO);
        public Task<ApiResponse<DepartmentResponseDTO>> UpdateDepartmentAsync(int departmentId, DepartmentAddEditDTO departmentRequestDTO);
        public Task<ApiResponse<bool>> DeleteDepartmentAsync(int departmentId);
    }
}
