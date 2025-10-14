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
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Full name is required.")
                .GreaterThan(0).WithMessage("The ID m");
        }
    }
}
