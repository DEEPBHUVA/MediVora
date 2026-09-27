using FluentValidation;

namespace MediVora.DTO
{
    #region DoctorAddDTO
    public class DoctorAddDTO
    {
        // When a Doctor Profile is created, automatically create a corresponding User Table entry with the appropriate Doctor role.
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        // 
        public int DepartmentID { get; set; }
        public string DoctorCode { get; set; }
        public string Qualification { get; set; }
        public string Specialization { get; set; }
        public string MedicalRegistrationNo { get; set; }
        public decimal ConsultationFee { get; set; }
        public bool IsActive { get; set; }
        public string ProfileImagePath { get; set; } = string.Empty;
        public IFormFile ProfileImage { get; set; }
    }


    public class DoctorAddValidator : AbstractValidator<DoctorAddDTO>
    {
        public DoctorAddValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100).WithMessage("First name is required and cannot exceed 100 characters.");
            RuleFor(x => x.MiddleName).MaximumLength(100).WithMessage("Middle name cannot exceed 100 characters.");
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100).WithMessage("Last name is required and cannot exceed 100 characters.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150).WithMessage("Please enter a valid email address.");
            RuleFor(x => x.MobileNo).NotEmpty().Matches(@"^[0-9]{10}$").WithMessage("Mobile number must contain exactly 10 digits.");
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(4).MaximumLength(50);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(100).Matches("[A-Z]").Matches("[a-z]").Matches("[0-9]").Matches(@"[\W_]").WithMessage("Password must contain uppercase, lowercase, number and special character.");
            RuleFor(x => x.DepartmentID).GreaterThan(0).WithMessage("Please select a valid department.");
            RuleFor(x => x.DoctorCode).NotEmpty().MaximumLength(50).WithMessage("Doctor code is required and cannot exceed 50 characters.");
            RuleFor(x => x.Qualification).NotEmpty().MaximumLength(200).WithMessage("Qualification is required and cannot exceed 200 characters.");
            RuleFor(x => x.Specialization).NotEmpty().MaximumLength(200).WithMessage("Specialization is required and cannot exceed 200 characters.");
            RuleFor(x => x.MedicalRegistrationNo).NotEmpty().MaximumLength(100).WithMessage("Medical registration number is required and cannot exceed 100 characters.");
            RuleFor(x => x.ConsultationFee).GreaterThanOrEqualTo(0).WithMessage("Consultation fee cannot be negative.");
            RuleFor(x => x.ProfileImage).Must(file => file == null || file.Length <= 2 * 1024 * 1024).WithMessage("Profile image size cannot exceed 5MB.");
        }
    }
    #endregion


    #region DoctorEditDTO
    public class DoctorEditDTO
    {
        public int DepartmentID { get; set; }
        public string DoctorCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Qualification { get; set; }
        public string Specialization { get; set; }
        public string MedicalRegistrationNo { get; set; }
        public decimal ConsultationFee { get; set; }
        public string ProfileImagePath { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }

    public class DoctorEditValidator : AbstractValidator<DoctorEditDTO>
    {
        public DoctorEditValidator()
        {
            RuleFor(x => x.DepartmentID).GreaterThan(0).WithMessage("Please select a valid department.");
            RuleFor(x => x.DoctorCode).NotEmpty().MaximumLength(50).WithMessage("Doctor code is required and cannot exceed 50 characters.");
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100).WithMessage("First name is required and cannot exceed 100 characters.");
            RuleFor(x => x.MiddleName).MaximumLength(100).WithMessage("Middle name cannot exceed 100 characters.");
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100).WithMessage("Last name is required and cannot exceed 100 characters.");
            RuleFor(x => x.Qualification).NotEmpty().MaximumLength(200).WithMessage("Qualification is required and cannot exceed 200 characters.");
            RuleFor(x => x.Specialization).NotEmpty().MaximumLength(200).WithMessage("Specialization is required and cannot exceed 200 characters.");
            RuleFor(x => x.MedicalRegistrationNo).NotEmpty().MaximumLength(100).WithMessage("Medical registration number is required and cannot exceed 100 characters.");
            RuleFor(x => x.ConsultationFee).GreaterThanOrEqualTo(0).WithMessage("Consultation fee cannot be negative.");
            //RuleFor(x => x.ProfileImagePath).MaximumLength(200).WithMessage("Profile image path cannot exceed 200 characters.");
        }
    }
    #endregion


    #region DoctorSearchFilterDTO
    public class DoctorSearchFilterDTO
    {
        public int? DepartmentID { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public decimal? MinConsultationFee { get; set; }
        public decimal? MaxConsultationFee { get; set; }
    }
    #endregion

    #region DoctorResponseDTO
    public class DoctorResponseDTO
    {
        public int DoctorID { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
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
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
    #endregion
}
