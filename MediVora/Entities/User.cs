using System.Data;

namespace MediVora.Entities
{
    public class User : EntityBase
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public virtual Roles Roles { get; set; }
        public virtual ICollection<Doctors> Doctors { get; set; }
        public virtual ICollection<Patients> Patients { get; set; }
    }
}
