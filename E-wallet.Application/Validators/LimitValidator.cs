using E_wallet.Application.Dtos.Request;
using E_wallet.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Validators
{
    public class LimitValidator : AbstractValidator<LimitRequest>
    {
        public LimitValidator()
        {
            RuleFor(x => x.amount)
                .NotEmpty().WithMessage("Amount is required.")
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.scope)
                .IsInEnum().WithMessage("Scope is required.")
                .Must(scope => scope == LimitScope.DAILY || scope == LimitScope.WEEKLY || scope == LimitScope.MONTHLY)
                .WithMessage("Scope must be either 'daily', 'weekly', or 'monthly'.");

            RuleFor(x => x.type)
                .IsInEnum().WithMessage("Type is required.")
                .Must(type => type == LimitType.AMOUNT || type == LimitType.TRANSACTION_COUNT )
                .WithMessage("Type must be either 'Amount', or 'TRANSACTION_COUNT'.");  
        }
    }
}
