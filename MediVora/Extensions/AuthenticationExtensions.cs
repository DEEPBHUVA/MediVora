using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json;

namespace MediVora.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // Add authentication services here
            var vJwtSettings = configuration.GetSection("JWT");

            var vKey = vJwtSettings["Key"];
            var vIssuer = vJwtSettings["Issuer"];
            var vAudience = vJwtSettings["Audience"];
            var vClockSkew = TimeSpan.Zero;

            if (string.IsNullOrEmpty(vKey) || vKey.Length < 32)
            {
                throw new Exception($"JWT key must be at least 32 characters long. Current length: {vKey?.Length ?? 0}");
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,

                    ClockSkew = TimeSpan.FromSeconds(30),

                    ValidIssuer = vIssuer,
                    ValidAudience = vAudience,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(vKey)),
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var result = JsonSerializer.Serialize(new
                        {
                            success = false,
                            message = "Unauthorized: Token is missing or invalid"
                        });

                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";

                        var result = JsonSerializer.Serialize(new
                        {
                            success = false,
                            message = "Forbidden: You do not have permission to access this resource"
                        });
                        return context.Response.WriteAsync(result);
                    }
                };
            });

            return services;
        }
    }
}
