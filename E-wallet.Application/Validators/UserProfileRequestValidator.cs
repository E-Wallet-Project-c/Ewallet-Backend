using E_wallet.Application.Dtos.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Validators
{
    public class UserProfileRequestValidator : AbstractValidator<UserProfileRequest>
    {
        private static readonly List<string> _countries = new()
        {
            "Algeria", "Bahrain", "Comoros", "Djibouti", "Egypt", "Iraq", "Jordan",
            "Kuwait", "Lebanon", "Libya", "Mauritania", "Morocco", "Oman", "Palestine",
            "Qatar", "Saudi Arabia", "Somalia", "Sudan", "Syria", "Tunisia", "United Arab Emirates",
            "Yemen"
        };
        public UserProfileRequestValidator()
        {
            // Phone validation
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^(077|078|079)\d{7}$").WithMessage("Invalid phone number format.");

            // Date of Birth validation
            RuleFor(x => x.DateOfBirth)
                            .NotEmpty().WithMessage("Date of birth is required.") // ensures not null
                            .Must(BeAValidDate).WithMessage("Date of birth must be a valid date and cannot be in the future.");

            // Country validation
            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.")
                .NotNull().WithMessage("Country is required.")
                .Must(country => _countries.Contains(country));
        }

        private bool BeAValidDate(DateOnly? date)
        {
            return date.HasValue && date.Value <= DateOnly.FromDateTime(DateTime.Today);
        }
    }
}
