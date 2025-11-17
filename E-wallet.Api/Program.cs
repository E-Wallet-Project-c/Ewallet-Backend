

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
using System.Text.Json.Serialization;

using E_wallet.Api.Midleware;
using E_wallet.Api.Exceptions;
namespace E_wallet.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddConectionString(builder.Configuration);
            builder.Services.AddConnections();
            builder.Services.AddEwalletServices();
            builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            builder.Services.AddAuthentiactionAndAuthorization(builder.Configuration);
            builder.Services.AddApiServices();

         
            var app = builder.Build();

            app.UseExceptionHandler();

            app.UseAuthentication();
            app.UseAuthorization();
       
            app.UseSwagger();
            app.UseSwaggerUI();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-wallet Api V1");

            //    c.RoutePrefix = string.Empty;
            //}
            //);

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseMiddleware<ResponseWrapperMiddleware>();

            app.MapControllers(); 

            app.MapHub<E_wallet.Applications.Hubs.AppHub>("/hubs/AppHub");


            app.Run();
        }
    }
}
