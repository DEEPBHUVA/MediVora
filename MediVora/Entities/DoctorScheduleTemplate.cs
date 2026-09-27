namespace MediVora.Entities
{
    public class DoctorScheduleTemplate : EntityBase
    {
        public int DoctorScheduleTemplateID { get; set; }
        public int DoctorID { get; set; }
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

        public virtual Doctors Doctor { get; set; }
    }
}
