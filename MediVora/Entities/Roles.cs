namespace MediVora.Entities
{
    public class Roles : EntityBase
    {
        public int RoleID { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }

        public virtual ICollection<User> Users { get; set; }

    }
}
