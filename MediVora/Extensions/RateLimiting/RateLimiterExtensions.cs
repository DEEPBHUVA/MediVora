using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using System.Threading.RateLimiting;

namespace MediVora.Extensions.RateLimiting
{
    public static class RateLimiterExtensions
    {
        public static IServiceCollection AddApplicationRateLimiting(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<RateLimitOptions>(configuration.GetSection("RateLimiting"));

            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                ConfigureRejectionHandler(options);

                ConfigureGlobalLimiter(options, configuration);

                ConfigureLoginPolicy(options, configuration);

                ConfigureRegisterPolicy(options, configuration);

                ConfigureDoctorSearchPolicy(options, configuration);

                ConfigureGeneralApiPolicy(options, configuration);

                ConfigureBookingPolicy(options, configuration);

                ConfigureSlidingWindowPolicy(options, configuration);

                ConfigureTokenBucketPolicy(options, configuration);

                ConfigureConcurrencyPolicy(options, configuration);
            });

            return services;
        }


        // ============================================================
        // REJECTION HANDLER
        // ============================================================

        private static void ConfigureRejectionHandler(RateLimiterOptions options)
        {
            options.OnRejected = async (context, cancellationToken) =>
            {
                var httpContext = context.HttpContext;

                var logger = httpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("RateLimiter");

                var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                var path = httpContext.Request.Path;

                logger.LogWarning("Rate limit exceeded. IP: {IP}, Path: {Path}", ip, path);

                httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

                httpContext.Response.ContentType = "application/json";

                httpContext.Response.Headers.RetryAfter = "60";

                await httpContext.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    statusCode = 429,
                    message = "Too many requests. Please try again later.",
                    retryAfterSeconds = 60
                }, cancellationToken);
            };
        }


        // ============================================================
        // GLOBAL LIMITER
        // ============================================================

        private static void ConfigureGlobalLimiter(RateLimiterOptions options, IConfiguration configuration)
        {
            var settings = configuration.GetSection("RateLimiting:Global").Get<GlobalRateLimitOptions>() ?? new GlobalRateLimitOptions { PermitLimit = 1000, WindowSeconds = 60 };

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = settings.PermitLimit,
                    Window = TimeSpan.FromSeconds(settings.WindowSeconds),
                    QueueLimit = 0,
                    AutoReplenishment = true
                });
            });
        }


        // ============================================================
        // LOGIN
        // IP BASED
        // ============================================================

        private static void ConfigureLoginPolicy(RateLimiterOptions options, IConfiguration configuration)
        {
            var settings = GetFixedWindowOptions(configuration, "RateLimiting:Login");

            options.AddPolicy(RateLimitPolicies.Login, httpContext =>
            {
                var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter($"login-ip:{ip}", _ => CreateFixedWindow(settings));
            });
        }


        // ============================================================
        // REGISTER
        // IP BASED
        // ============================================================

        private static void ConfigureRegisterPolicy(RateLimiterOptions options, IConfiguration configuration)
        {
            var settings = GetFixedWindowOptions(configuration, "RateLimiting:Register");

            options.AddPolicy(RateLimitPolicies.Register, httpContext =>
            {
                var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter($"register-ip:{ip}", _ => CreateFixedWindow(settings));
            });
        }


        // ============================================================
        // DOCTOR SEARCH
        // USER/IP BASED
        // ============================================================

        private static void ConfigureDoctorSearchPolicy(RateLimiterOptions options, IConfiguration configuration)
        {
            var settings = GetFixedWindowOptions(configuration, "RateLimiting:DoctorSearch");

            options.AddPolicy(RateLimitPolicies.DoctorSearch, httpContext =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                var partitionKey = !string.IsNullOrWhiteSpace(userId) ? $"user:{userId}" : $"ip:{ip}";

                return RateLimitPartition.GetFixedWindowLimiter(partitionKey, _ => CreateFixedWindow(settings));
            });
        }


        // ============================================================
        // GENERAL API
        // USER/IP BASED
        // ============================================================

        private static void ConfigureGeneralApiPolicy(RateLimiterOptions options, IConfiguration configuration)
        {
            var settings = GetFixedWindowOptions(configuration, "RateLimiting:GeneralApi");

            options.AddPolicy(RateLimitPolicies.GeneralApi, httpContext =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                var partitionKey = !string.IsNullOrWhiteSpace(userId) ? $"user:{userId}" : $"ip:{ip}";

                return RateLimitPartition.GetFixedWindowLimiter(partitionKey, _ => CreateFixedWindow(settings));
            });
        }


        // ============================================================
        // APPOINTMENT BOOKING
        // USER BASED
        // ============================================================

        private static void ConfigureBookingPolicy(RateLimiterOptions options, IConfiguration configuration)
        {
            var settings = GetFixedWindowOptions(configuration, "RateLimiting:Booking");

            options.AddPolicy(RateLimitPolicies.AppointmentBooking, httpContext =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                var partitionKey = !string.IsNullOrWhiteSpace(userId) ? $"booking-user:{userId}" : $"booking-ip:{ip}";

                return RateLimitPartition.GetFixedWindowLimiter(partitionKey, _ => CreateFixedWindow(settings));
            });
        }


        // ============================================================
        // SLIDING WINDOW
        // ============================================================

        private static void ConfigureSlidingWindowPolicy(RateLimiterOptions options, IConfiguration configuration)
        {
            var settings = configuration.GetSection("RateLimiting:SlidingApi").Get<SlidingWindowOptions>() ?? new SlidingWindowOptions { PermitLimit = 100, WindowSeconds = 60, SegmentsPerWindow = 6 };

            options.AddPolicy(RateLimitPolicies.SlidingApi, httpContext =>
            {
                var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetSlidingWindowLimiter($"sliding:{ip}", _ => new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = settings.PermitLimit,
                    Window = TimeSpan.FromSeconds(settings.WindowSeconds),
                    SegmentsPerWindow = settings.SegmentsPerWindow,
                    QueueLimit = 0,
                    AutoReplenishment = true
                });
            });
        }


        // ============================================================
        // TOKEN BUCKET
        // ============================================================

        private static void ConfigureTokenBucketPolicy(RateLimiterOptions options, IConfiguration configuration)
        {
            var settings = configuration.GetSection("RateLimiting:TokenBucket").Get<TokenBucketOptions>() ?? new TokenBucketOptions { TokenLimit = 20, TokensPerPeriod = 10, ReplenishmentPeriodSeconds = 30, QueueLimit = 0 };

            options.AddPolicy(RateLimitPolicies.TokenBucket, httpContext =>
            {
                var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetTokenBucketLimiter($"token:{ip}", _ => new TokenBucketRateLimiterOptions
                {
                    TokenLimit = settings.TokenLimit,
                    TokensPerPeriod = settings.TokensPerPeriod,
                    ReplenishmentPeriod = TimeSpan.FromSeconds(settings.ReplenishmentPeriodSeconds),
                    QueueLimit = settings.QueueLimit,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    AutoReplenishment = true
                });
            });
        }


        // ============================================================
        // CONCURRENCY LIMITER
        // ============================================================

        private static void ConfigureConcurrencyPolicy(RateLimiterOptions options, IConfiguration configuration)
        {
            var settings = configuration.GetSection("RateLimiting:ExpensiveOperation").Get<ConcurrencyOptions>() ?? new ConcurrencyOptions { PermitLimit = 5, QueueLimit = 0 };

            options.AddPolicy(RateLimitPolicies.ExpensiveOperation, httpContext =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var partitionKey = !string.IsNullOrWhiteSpace(userId) ? $"expensive-user:{userId}" : $"expensive-ip:{httpContext.Connection.RemoteIpAddress}";

                return RateLimitPartition.GetConcurrencyLimiter(partitionKey, _ => new ConcurrencyLimiterOptions
                {
                    PermitLimit = settings.PermitLimit,
                    QueueLimit = settings.QueueLimit,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                });
            });
        }


        // ============================================================
        // HELPERS
        // ============================================================

        private static FixedWindowOptions GetFixedWindowOptions(IConfiguration configuration, string section)
        {
            return configuration.GetSection(section).Get<FixedWindowOptions>() ?? new FixedWindowOptions { PermitLimit = 100, WindowSeconds = 60 };
        }


        private static FixedWindowRateLimiterOptions CreateFixedWindow(FixedWindowOptions settings)
        {
            return new FixedWindowRateLimiterOptions
            {
                PermitLimit = settings.PermitLimit,
                Window = TimeSpan.FromSeconds(settings.WindowSeconds),
                QueueLimit = 0,
                AutoReplenishment = true
            };
        }
    }
}
