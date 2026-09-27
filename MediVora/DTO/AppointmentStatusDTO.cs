using FluentValidation;

namespace MediVora.DTO
{
    public class AppointmentStatusAddEditDTO
    {
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public bool IsActive { get; set; }
    }

    public class AppointmentStatusAddEditDTOValidator : AbstractValidator<AppointmentStatusAddEditDTO>
    {
        public AppointmentStatusAddEditDTOValidator()
        {
            RuleFor(x => x.StatusCode)
                .NotEmpty()
                .WithMessage("Status Code is required.")
                .MaximumLength(50)
                .WithMessage("Status Code cannot exceed 50 characters.");

            RuleFor(x => x.StatusName)
                .NotEmpty()
                .WithMessage("Status Name is required.")
                .MaximumLength(100)
                .WithMessage("Status Name cannot exceed 100 characters.");
        }
    }


    public class AppointmentStatusResponseDTO
    {
        public int AppointmentStatusID { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }


    public class AppointmentStatusDropdownDTO
    {
        public int AppointmentStatusID { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string StatusDisplayName { get; set; }
    }
}
