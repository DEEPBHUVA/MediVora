namespace MediVora.Entities
{
    public class Patients : EntityBase
    {
        public int PatientID { get; set; }
        public int UserID { get; set; }
        public string PatientCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }
        public bool IsActive { get; set; }

        public User User { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
