using E_wallet.Application.Dtos.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Validators
{
    public class NewPasswordValidator: AbstractValidator<NewPasswordrequest>
    {
        public NewPasswordValidator()
        {
            RuleFor(x => x.newPassword)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                    .MaximumLength(25).WithMessage("Password must not exceed 25 characters.")
                    .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                    .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                    .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                    .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.confirmPassword).NotEmpty().WithMessage("Password is required.")
                .Equal(x => x.newPassword).WithMessage("Passwords do not match.");



        }



    }
}
