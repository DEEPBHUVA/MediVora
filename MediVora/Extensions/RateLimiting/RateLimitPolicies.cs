namespace MediVora.Extensions.RateLimiting
{
    public static class RateLimitPolicies
    {
        public const string Global = "global";
        public const string Login = "login";
        public const string Register = "register";
        public const string DoctorSearch = "doctor-search";
        public const string GeneralApi = "general-api";
        public const string AppointmentBooking = "appointment-booking";
        public const string SlidingApi = "sliding-api";
        public const string TokenBucket = "token-bucket";
        public const string ExpensiveOperation = "expensive-operation";
    }
}
