namespace MediVora.Entities
{
    public class Department : EntityBase
    {
        public int DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }

        public virtual ICollection<Doctors> Doctors { get; set; }
    }
}
