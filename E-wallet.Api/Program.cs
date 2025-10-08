
using E_wallet.Application.Validators;
using FluentValidation;

namespace E_wallet.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddConnections();
            builder.Services.AddValidatorsFromAssembly(typeof(RegisterRequestValidator).Assembly, includeInternalTypes: true);
            builder.Services.AddValidatorsFromAssembly(typeof(UserProfileRequestValidator).Assembly, includeInternalTypes: true);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();

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
