namespace MediVora.DTO
{
    public class UserDTO
    {
    }

    #region UserResponseDTO
    public class UserResponseDTO
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
    #endregion
}
