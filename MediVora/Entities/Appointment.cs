namespace MediVora.Entities
{
    public class Appointment : EntityBase
    {
        public int AppointmentID { get; set; }
        public string AppointmentNo { get; set; }
        public int DoctorScheduleID { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        public int AppointmentStatusID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string AppointmentType { get; set; }
        public string Reason { get; set; }
        public string PatientRemarks { get; set; }
        public string DoctorRemarks { get; set; }
        public DateTime BookedDate { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public DateTime? CheckedInDate { get; set; }
        public DateTime? StartedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string CancellationReason { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }


        public DoctorSchedule DoctorSchedule { get; set; }
        public Doctors Doctor { get; set; }
        public Patients Patient { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
    }
}
