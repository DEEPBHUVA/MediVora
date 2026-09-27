namespace MediVora.Interfaces
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
        bool? IsAdmin { get; }
        bool? IsDoctor { get; }
        bool? IsPatient { get; }
    }
}
