using E_wallet.Application.Dtos.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Validators
{
    public sealed class RegisterRequestValidator : AbstractValidator<UserRegisterRequest>
    {
        public RegisterRequestValidator()
        {
            // FullName validation
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(50).WithMessage("The name must not exceed 50 character")
                .MinimumLength(3).WithMessage("The name should be at least 3 character");


            // Email validation

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(50).WithMessage("The email must not exceed 50 characters.")
                .Matches(@"^[A-Za-z0-9._%+-]+@(gmail\.com|yahoo\.com|outlook\.com)$")
                .WithMessage("Email must be valid and end with gmail.com, yahoo.com, or outlook.com.");

            //// Date of Birth validation
            //RuleFor(x => x.DateOfBirth)
            //    .NotEmpty().WithMessage("Date of birth is required.")
            //    .Must(BeAValidDate).WithMessage("Date of birth must be a valid date and cannot be in the future.");
            
            //// Phone validation
            //RuleFor(x => x.Phone)
            //    .NotEmpty().WithMessage("Phone number is required.")
            //    .Matches(@"^(077|078|079)\d{7}$").WithMessage("Invalid phone number format.");

            // Password validation
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(25).WithMessage("Password must not exceed 25 characters.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            // Confirm Password validation
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required.")
                .Equal(x => x.Password).WithMessage("Passwords do not match.");

        }

        private bool BeAValidDate(DateOnly date)
        {
            return date <= DateOnly.FromDateTime(DateTime.Today);
        }
    }
}
