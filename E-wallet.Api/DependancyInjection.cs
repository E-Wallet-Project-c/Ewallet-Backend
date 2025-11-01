using E_wallet.Application.Interfaces;
using E_wallet.Application.Mappers;
using E_wallet.Application.Services;
using E_wallet.Application.Validators;
using E_wallet.Domain.Context;
using E_wallet.Domain.Interfaces;
using E_wallet.Infrastrucure.Helpers;
using E_wallet.Infrastrucure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace E_wallet.Api
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddEwalletServices(this IServiceCollection services)
        {
            // Register your services here:
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            // Add MailingHelper as a singleton service
            services.AddScoped<E_wallet.Domain.IHelpers.IEmailHelper, MailingHelper>();
            //Create Profile service and repository
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            // Repository for wallet
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<WalletMapper>();
            // Service for wallet
            services.AddScoped<IWalletService, WalletService>();
            //Jwt Register
            services.AddScoped<IJwtService, JwtService>();
            //mapper 
            services.AddSingleton<ProfileMapper>();
            //Create UserBankAccount service and repository
            services.AddScoped<IUserBankAccountService, UserBankAccountService>();
            services.AddScoped<IUserBankAccountRepository, UserBankAccountRepository>();
            return services;
        }
        public static IServiceCollection AddAuthentiactionAndAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            //Jwt Authentication
            services.AddAuthentication(options =>
              {
                  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

              }
              ).AddJwtBearer(options =>
              {
                  var jwtSettings = configuration.GetSection("JwtSettings");

                  options.TokenValidationParameters = new()
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ClockSkew = TimeSpan.Zero,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = jwtSettings["validIssuer"],
                      ValidAudience = jwtSettings["validAudience"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["secretKey"]!))

                  };
              });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            });

            return services;
        }
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddOpenApi();
            services.AddSwaggerGen();
            services.AddFluentValidationAutoValidation();

            services.AddValidatorsFromAssembly(typeof(RegisterRequestValidator).Assembly, includeInternalTypes: true);
            services.AddValidatorsFromAssembly(typeof(UserProfileRequestValidator).Assembly, includeInternalTypes: true);

            return services;

        }
        public static IServiceCollection AddConectionString(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}