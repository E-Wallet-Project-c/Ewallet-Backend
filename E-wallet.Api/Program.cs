
using E_wallet.Application.Interfaces;
using E_wallet.Application.Services;
using E_wallet.Application.Validators;
using E_wallet.Domain.Context;
using E_wallet.Domain.Interfaces;
using E_wallet.Infrastrucure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


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
            builder.Services.AddValidatorsFromAssembly(typeof(RegisterRequestValidator).Assembly, includeInternalTypes: true);
            builder.Services.AddValidatorsFromAssembly(typeof(UserProfileRequestValidator).Assembly, includeInternalTypes: true);
            // Register your services here:
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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
