namespace MediVora.Extensions.RateLimiting
{
    public class RateLimitOptions
    {
        public GlobalRateLimitOptions Global { get; set; } = new();

        public FixedWindowOptions Login { get; set; } = new();

        public FixedWindowOptions Register { get; set; } = new();

        public FixedWindowOptions DoctorSearch { get; set; } = new();

        public FixedWindowOptions GeneralApi { get; set; } = new();

        public FixedWindowOptions AppointmentBooking { get; set; } = new();

        public SlidingWindowOptions SlidingApi { get; set; } = new();

        public TokenBucketOptions TokenBucket { get; set; } = new();

        public ConcurrencyOptions ExpensiveOperation { get; set; } = new();
    }


    public class GlobalRateLimitOptions
    {
        public int PermitLimit { get; set; }
        public int WindowSeconds { get; set; }
    }


    public class FixedWindowOptions
    {
        public int PermitLimit { get; set; }
        public int WindowSeconds { get; set; }
    }


    public class SlidingWindowOptions
    {
        public int PermitLimit { get; set; }
        public int WindowSeconds { get; set; }
        public int SegmentsPerWindow { get; set; }
    }


    public class TokenBucketOptions
    {
        public int TokenLimit { get; set; }
        public int TokensPerPeriod { get; set; }
        public int ReplenishmentPeriodSeconds { get; set; }
        public int QueueLimit { get; set; }
    }


    public class ConcurrencyOptions
    {
        public int PermitLimit { get; set; }
        public int QueueLimit { get; set; }
    }
}
