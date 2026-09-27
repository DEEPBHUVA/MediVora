using FluentValidation;

namespace MediVora.DTO
{
    public class PatientAppointmentAddDTO
    {
        public int DoctorScheduleID { get; set; }
        public int DoctorID { get; set; }
        public string AppointmentType { get; set; }
        public string Reason { get; set; }
    }

    public class AppointmentAddDTOValidator : AbstractValidator<PatientAppointmentAddDTO>
    {
        public AppointmentAddDTOValidator()
        {
            RuleFor(x => x.DoctorScheduleID)
                .GreaterThan(0)
                .WithMessage("Doctor Schedule ID must be greater than 0.");

            RuleFor(x => x.DoctorID)
                .GreaterThan(0)
                .WithMessage("Doctor ID must be greater than 0.");

            RuleFor(x => x.AppointmentType)
                .NotEmpty()
                .WithMessage("Appointment Type is required.")
                .MaximumLength(30)
                .WithMessage("Appointment Type cannot exceed 30 characters.");

            RuleFor(x => x.Reason)
                .MaximumLength(1000)
                .WithMessage("Reason cannot exceed 1000 characters.");
        }
    }




    public class AdminAppointmentAddDTO
    {
        public int DoctorScheduleID { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        public string AppointmentType { get; set; }
        public string Reason { get; set; }
    }

    public class AdminAppointmentAddDTOValidator : AbstractValidator<AdminAppointmentAddDTO>
    {
        public AdminAppointmentAddDTOValidator()
        {
            RuleFor(x => x.DoctorScheduleID)
                .GreaterThan(0)
                .WithMessage("Doctor Schedule ID must be greater than 0.");

            RuleFor(x => x.DoctorID)
                .GreaterThan(0)
                .WithMessage("Doctor ID must be greater than 0.");

            RuleFor(x => x.PatientID)
               .GreaterThan(0)
               .WithMessage("Patient ID must be greater than 0.");

            RuleFor(x => x.AppointmentType)
                .NotEmpty()
                .WithMessage("Appointment Type is required.")
                .MaximumLength(30)
                .WithMessage("Appointment Type cannot exceed 30 characters.");

            RuleFor(x => x.Reason)
                .MaximumLength(1000)
                .WithMessage("Reason cannot exceed 1000 characters.");
        }
    }


    public class AppointmentCancelDTO
    {
        public int AppointmentID { get; set; }
        public string CancellationReason { get; set; }
    }

    public class AppointmentCancelDTOValidator : AbstractValidator<AppointmentCancelDTO>
    {
        public AppointmentCancelDTOValidator()
        {
            RuleFor(x => x.AppointmentID)
                .GreaterThan(0)
                .WithMessage("Appointment must be greater than 0.");

            RuleFor(x => x.CancellationReason)
                .MaximumLength(1000)
                .WithMessage("Cancellation Reason cannot exceed 1000 characters.");
        }
    }

    public class AppointmentSearchFilterDTO
    {
        public string? ViewType { get; set; }
        public DateTime? Date { get; set; }
        public int? AppointmentStatusID { get; set; }
    }

    public class AppointmentSearchFilterDTOValidator : AbstractValidator<AppointmentSearchFilterDTO>
    {
        public AppointmentSearchFilterDTOValidator()
        {
            RuleFor(x => x.Date)
                .Must(x => !x.HasValue || x.Value.Date >= DateTime.MinValue.Date)
                .WithMessage("Invalid appointment date.");

            RuleFor(x => x.AppointmentStatusID)
                .GreaterThan(0)
                .When(x => x.AppointmentStatusID.HasValue)
                .WithMessage("Appointment Status ID must be greater than 0.");
        }
    }


    public class DoctorAppointmentSearchFilterDTO
    {
        public string? ViewType { get; set; }
        public DateTime? Date { get; set; }
        public int AppointmentStatusID { get; set; }
    }

    public class DoctorAppointmentSearchFilterDTOValidator : AbstractValidator<DoctorAppointmentSearchFilterDTO>
    {
        public DoctorAppointmentSearchFilterDTOValidator()
        {
            RuleFor(x => x.ViewType).Must(x => x == null || x.Equals("Today", StringComparison.OrdinalIgnoreCase) || x.Equals("Upcoming", StringComparison.OrdinalIgnoreCase) || x.Equals("History", StringComparison.OrdinalIgnoreCase))
                .WithMessage("ViewType must be either Today, Upcoming, or History.");

            RuleFor(x => x.AppointmentStatusID)
                .GreaterThan(0)
                .When(x => x.AppointmentStatusID != 0)
                .WithMessage("Appointment Status ID must be greater than 0.");
        }
    }


    public class AppointmentStatusUpdateDTO
    {
        public int AppointmentID { get; set; }
        public int AppointmentStatusID { get; set; }
    }

    public class AppointmentStatusUpdateDTOValidator : AbstractValidator<AppointmentStatusUpdateDTO>
    {
        public AppointmentStatusUpdateDTOValidator()
        {
            RuleFor(x => x.AppointmentID).NotEmpty().WithName("Appointment");

            RuleFor(x => x.AppointmentStatusID)
                .GreaterThan(0)
                .When(x => x.AppointmentStatusID != 0)
                .WithMessage("Appointment Status ID must be greater than 0.");
        }
    }




    public class AppointmentResponseDTO
    {
        public int AppointmentID { get; set; }
        public string AppointmentNo { get; set; }
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string DoctorCode { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public string PatientCode { get; set; }
        public string MobileNo { get; set; }
        public int AppointmentStatusID { get; set; }
        public string AppointmentStatusCode { get; set; }
        public string AppointmentStatusName { get; set; }
        public string AppointmentType { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DateTime BookedDate { get; set; }
        public string Reason { get; set; }
        public string PatientRemarks { get; set; }
        public string DoctorRemarks { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public DateTime? CheckedInDate { get; set; }
        public DateTime? StartedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string CancellationReason { get; set; }
    }
}
