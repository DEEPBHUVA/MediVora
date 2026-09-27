using FluentValidation;

namespace MediVora.DTO
{
    public class DoctorScheduleTemplateAddEditDTO
    {
        // public int DoctorID { get; set; }
        public int DayOfWeek { get; set; }
        public string DayOfWeekName { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int SlotDurationInMinutes { get; set; }
        public int MaxAppointmentsPerSlot { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class DoctorScheduleTemplateAddEditDTOValidator : AbstractValidator<DoctorScheduleTemplateAddEditDTO>
    {
        public DoctorScheduleTemplateAddEditDTOValidator()
        {
            //RuleFor(x => x.DoctorID).NotEmpty().WithMessage("Doctor ID is required.");
            RuleFor(x => x.DayOfWeek).InclusiveBetween(1, 7).WithMessage("Day of week must be between 1 (Monday) and 7 (Sunday).");
            RuleFor(x => x.StartTime).NotEmpty().WithMessage("Start time is required.");
            RuleFor(x => x.EndTime).NotEmpty().WithMessage("End time is required.");
            RuleFor(x => x.SlotDurationInMinutes).GreaterThan(0).WithMessage("Slot duration must be greater than 0.");
            RuleFor(x => x.MaxAppointmentsPerSlot).GreaterThan(0).WithMessage("Max appointments per slot must be greater than 0.");
            RuleFor(x => x.EffectiveFromDate).NotEmpty().WithMessage("Effective from date is required.");
            RuleFor(x => x.IsActive).NotNull().WithMessage("IsActive is required.");
        }
    }



    public class DoctorScheduleTemplateResponseDTO
    {
        public int DoctorScheduleTemplateID { get; set; }
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string DoctorCode { get; set; }
        public string MedicalRegistrationNo { get; set; } = string.Empty;
        public decimal ConsultationFee { get; set; } = decimal.Zero;
        public int DayOfWeek { get; set; }
        public string DayOfWeekName { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int SlotDurationInMinutes { get; set; }
        public int MaxAppointmentsPerSlot { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
