using E_wallet.Application.Dtos.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Validators
{
    public class ForgetPasswordEmailvalidator : AbstractValidator<ForgetPasswordEmailrequest>
    {
        public ForgetPasswordEmailvalidator()
        {
            // FullName validation

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(50).WithMessage("The email must not exceed 50 characters.")
                .Matches(@"^[A-Za-z0-9._%+-]+@(gmail\.com|yahoo\.com|outlook\.com)$")
                .WithMessage("Email must be valid and end with gmail.com, yahoo.com, or outlook.com.");
        }
    }
}
