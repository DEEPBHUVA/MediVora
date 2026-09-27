namespace MediVora.Entities
{
    public class DoctorSchedule : EntityBase
    {
        public int DoctorScheduleID { get; set; }
        public int DoctorID { get; set; }
        public DateTime ScheduleDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int MaxAppointments { get; set; }
        public bool IsAvailable { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }

        public Doctors Doctor { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
