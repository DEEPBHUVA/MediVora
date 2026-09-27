using FluentValidation;

namespace MediVora.DTO
{
    public class DoctorScheduleAddEditDTO
    {
        public DateTime ScheduleDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int MaxAppointments { get; set; }
        public bool IsAvailable { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }

    public class DoctorScheduleAddEditDTOValidator : AbstractValidator<DoctorScheduleAddEditDTO>
    {
        public DoctorScheduleAddEditDTOValidator()
        {
            RuleFor(x => x.ScheduleDate).NotEmpty().WithMessage("Schedule date is required.");
            RuleFor(x => x.StartTime).NotEmpty().WithMessage("Start time is required.");
            RuleFor(x => x.EndTime).NotEmpty().WithMessage("End time is required.")
                .GreaterThan(x => x.StartTime).WithMessage("End time must be greater than start time.");
            RuleFor(x => x.MaxAppointments).GreaterThan(0).WithMessage("Max appointments must be greater than zero.");
            RuleFor(x => x.IsAvailable).NotEmpty().WithMessage("Is available is required.");
        }
    }



    public class GenerateDoctorScheduleDTO
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }

    public class GenerateDoctorScheduleDTOValidator : AbstractValidator<GenerateDoctorScheduleDTO>
    {
        public GenerateDoctorScheduleDTOValidator()
        {
            RuleFor(x => x.FromDate).NotEmpty().WithMessage("From date is required.");
            RuleFor(x => x.ToDate).NotEmpty().WithMessage("To date is required.")
                .GreaterThanOrEqualTo(x => x.FromDate).WithMessage("To date must be greater than or equal to from date.");
        }
    }



    public class DoctorScheduleResponseDTO
    {
        public int DoctorScheduleID { get; set; }
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string DoctorCode { get; set; }
        public string MedicalRegistrationNo { get; set; } = string.Empty;
        public decimal ConsultationFee { get; set; } = decimal.Zero;
        public DateTime ScheduleDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int MaxAppointments { get; set; }
        public bool IsAvailable { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
