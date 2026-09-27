using FluentValidation;

namespace MediVora.DTO
{
    public class PatientAddDTO
    {
        public int RoleID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
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
    }

    public class PatientAddDTOValidator : AbstractValidator<PatientAddDTO>
    {
        public PatientAddDTOValidator()
        {
            RuleFor(x => x.RoleID).GreaterThan(0).WithMessage("Role is required.");
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(4).MaximumLength(50);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(100).Matches("[A-Z]").Matches("[a-z]").Matches("[0-9]").Matches(@"[\W_]").WithMessage("Password must contain uppercase, lowercase, number and special character.");
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100).WithMessage("First name is required and cannot exceed 100 characters.");
            RuleFor(x => x.MiddleName).MaximumLength(100).WithMessage("Middle name cannot exceed 100 characters.");
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100).WithMessage("Last name is required and cannot exceed 100 characters.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150).WithMessage("Please enter a valid email address.");
            RuleFor(x => x.MobileNo).NotEmpty().Matches(@"^[0-9]{10}$").WithMessage("Mobile number must contain exactly 10 digits.");
            RuleFor(x => x.DateOfBirth).LessThan(DateTime.Today).When(x => x.DateOfBirth.HasValue).WithMessage("Date of birth must be in the past.");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Gender is required.").Must(x => new[] { "Male", "Female", "Other" }.Contains(x)).WithMessage("Invalid gender.");
            RuleFor(x => x.BloodGroup).Must(x => new[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" }.Contains(x)).When(x => !string.IsNullOrWhiteSpace(x.BloodGroup)).WithMessage("Invalid blood group.");
            RuleFor(x => x.Address).MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");
            RuleFor(x => x.City).MaximumLength(100).WithMessage("City cannot exceed 100 characters.");
            RuleFor(x => x.State).MaximumLength(100).WithMessage("State cannot exceed 100 characters.");
            RuleFor(x => x.PostalCode).Matches(@"^\d{6}$").When(x => !string.IsNullOrWhiteSpace(x.PostalCode)).WithMessage("Please enter a valid 6-digit postal code.");
            RuleFor(x => x.EmergencyContactName).MaximumLength(150).WithMessage("Emergency contact name cannot exceed 150 characters.");
            RuleFor(x => x.EmergencyContactNumber).Matches(@"^[6-9]\d{9}$").When(x => !string.IsNullOrWhiteSpace(x.EmergencyContactNumber)).WithMessage("Please enter a valid emergency contact number.");
            RuleFor(x => x.IsActive).NotEmpty().WithMessage("Is Active is required");

        }
    }


    public class PatientEditDTO
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }
    }

    public class PatientEditDTOValidator : AbstractValidator<PatientEditDTO>
    {
        public PatientEditDTOValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100).WithMessage("First name is required and cannot exceed 100 characters.");
            RuleFor(x => x.MiddleName).MaximumLength(100).WithMessage("Middle name cannot exceed 100 characters.");
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100).WithMessage("Last name is required and cannot exceed 100 characters.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150).WithMessage("Please enter a valid email address.");
            RuleFor(x => x.MobileNo).NotEmpty().Matches(@"^[0-9]{10}$").WithMessage("Mobile number must contain exactly 10 digits.");
            RuleFor(x => x.DateOfBirth).LessThan(DateTime.Today).When(x => x.DateOfBirth.HasValue).WithMessage("Date of birth must be in the past.");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Gender is required.").Must(x => new[] { "Male", "Female", "Other" }.Contains(x)).WithMessage("Invalid gender.");
            RuleFor(x => x.BloodGroup).Must(x => new[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" }.Contains(x)).When(x => !string.IsNullOrWhiteSpace(x.BloodGroup)).WithMessage("Invalid blood group.");
            RuleFor(x => x.Address).MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");
            RuleFor(x => x.City).MaximumLength(100).WithMessage("City cannot exceed 100 characters.");
            RuleFor(x => x.State).MaximumLength(100).WithMessage("State cannot exceed 100 characters.");
            RuleFor(x => x.PostalCode).Matches(@"^\d{6}$").When(x => !string.IsNullOrWhiteSpace(x.PostalCode)).WithMessage("Please enter a valid 6-digit postal code.");
            RuleFor(x => x.EmergencyContactName).MaximumLength(150).WithMessage("Emergency contact name cannot exceed 150 characters.");
            RuleFor(x => x.EmergencyContactNumber).Matches(@"^[6-9]\d{9}$").When(x => !string.IsNullOrWhiteSpace(x.EmergencyContactNumber)).WithMessage("Please enter a valid emergency contact number.");
        }
    }


    public class PatientResponseDTO
    {
        public int PatientID { get; set; }
        public int UserID { get; set; } = 0;
        public string PatientCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
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
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
