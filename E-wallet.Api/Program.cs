
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
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
             options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddConnections();

            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddValidatorsFromAssembly(typeof(RegisterRequestValidator).Assembly, includeInternalTypes: true);
            builder.Services.AddValidatorsFromAssembly(typeof(UserProfileRequestValidator).Assembly, includeInternalTypes: true);
            //mapper 
            builder.Services.AddSingleton<ProfileMapper>();
            // Register your services here:
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            // Add MailingHelper as a singleton service
            builder.Services.AddScoped<E_wallet.Domain.IHelpers.IEmailHelper, MailingHelper>();
            //Create Profile service and repository
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
            // Repository for wallet
            builder.Services.AddScoped<IWalletRepository, WalletRepository>();

            // Service for wallet
            builder.Services.AddScoped<IWalletService, WalletService>();
            //Jwt Register
            builder.Services.AddScoped<IJwtService, JwtService>();  

            //Jwt Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }
            ).AddJwtBearer(options =>
            {
                var jwtSettings = builder.Configuration.GetSection("JwtSettings");

                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["validIssuer"],
                    ValidAudience = jwtSettings["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["secretKey"]!))

                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            });

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI();
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
