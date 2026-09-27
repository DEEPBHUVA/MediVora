using FluentValidation;

namespace MediVora.DTO
{
    public class DepartmentAddEditDTO
    {
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class DepartmentAddEditDTOValidator : AbstractValidator<DepartmentAddEditDTO>
    {
        public DepartmentAddEditDTOValidator()
        {
            RuleFor(x => x.DepartmentName)
                .NotEmpty().WithMessage("Department name is required.")
                .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters.");
            RuleFor(x => x.DepartmentCode).NotEmpty().WithMessage("Department code is required.");
            RuleFor(x => x.IsActive).NotNull().WithMessage("IsActive is required.");

        }
    }


    public class DepartmentResponseDTO
    {
        public int DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
