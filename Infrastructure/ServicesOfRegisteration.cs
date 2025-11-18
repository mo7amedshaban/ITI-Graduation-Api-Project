using Core.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json;


namespace Infrastructure
{
    public static class ServicesOfRegisteration
    {
        public static IServiceCollection AddServiceRegisteration(this IServiceCollection services,
                                                                      IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind("jwtSettings", jwtSettings);
            services.AddSingleton(jwtSettings);

            // Identity setup
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.AllowedUserNameCharacters
                = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<AppDBContext>()
            .AddDefaultTokenProviders();

            //JWT Authentication setup
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //sechma
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = jwtSettings.ValidateAudience,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = jwtSettings.ValidateLifetime,
                    ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                    LifetimeValidator = (notBefore, expires, token, parameters) =>
                    {
                        return expires != null && expires > DateTime.UtcNow;
                    },
                    SaveSigninToken = true
                };

                // Comprehensive JWT Bearer Events
                options.Events = new JwtBearerEvents
                {
                    // Handle 401 - Unauthorized (invalid/missing token)
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var response = new
                        {
                            error = "Unauthorized",
                            message = "Authentication failed. Token is invalid, expired, or missing.",
                            statusCode = 401,
                            timestamp = DateTime.UtcNow
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    },

                    // Handle authentication failures
                    OnAuthenticationFailed = async context =>
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var response = new
                        {
                            error = "Authentication Failed",
                            message = "Token validation failed: " + context.Exception.Message,
                            statusCode = 401,
                            timestamp = DateTime.UtcNow
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    },

                    // Handle forbidden access (token valid but insufficient permissions)
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";

                        var response = new
                        {
                            error = "Forbidden",
                            message = "Access denied. You don't have permission to access this resource.",
                            statusCode = 403,
                            timestamp = DateTime.UtcNow
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }
                };
            });


            return services;
        }
    }
}

