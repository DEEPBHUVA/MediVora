using Microsoft.AspNetCore.Server.HttpSys;

namespace MediVora.Entities
{
    public class AppointmentStatus : EntityBase
    {
        public int AppointmentStatusID { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
