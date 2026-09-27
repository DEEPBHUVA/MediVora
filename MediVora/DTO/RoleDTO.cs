using FluentValidation;

namespace MediVora.DTO
{
    public class RoleAddEditDTO
    {
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public bool IsActive { get; set; }
    }

    public class RoleAddEditValidator : AbstractValidator<RoleAddEditDTO>
    {
        public RoleAddEditValidator() 
        { 
            RuleFor(x => x.RoleCode).NotEmpty().WithMessage("Role code is required.");
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("Role name is required.");
            RuleFor(x => x.RoleDescription).NotEmpty().WithMessage("Role description is required.");
            RuleFor(x => x.IsActive).NotNull().WithMessage("IsActive status is required.");
        }
    }

    #region RoleResponseDTO

    public class RoleResponseDTO
    {
        public int RoleID { get; set; }
        public string RoleCode { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string RoleDescription { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    #endregion
}
