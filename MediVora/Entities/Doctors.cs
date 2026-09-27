namespace MediVora.Entities
{
    public class Doctors : EntityBase
    {
        public int DoctorID { get; set; }
        public int UserID { get; set; }
        public int DepartmentID { get; set; }
        public string DoctorCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Qualification { get; set; }
        public string Specialization { get; set; }
        public string MedicalRegistrationNo { get; set; }
        public decimal ConsultationFee { get; set; }
        public string ProfileImagePath { get; set; }
        public bool IsActive { get; set; }
        public bool IsTrashed { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }

        public Department Department { get; set; }
        public User User { get; set; }
        public ICollection<DoctorScheduleTemplate> DoctorScheduleTemplates { get; set; }
        public ICollection<DoctorSchedule> DoctorSchedules { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
