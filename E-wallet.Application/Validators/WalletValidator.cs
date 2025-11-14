using E_wallet.Application.Dtos.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Validators
{
    public class WalletValidator : AbstractValidator<WalletRequest>
    {
        public WalletValidator() { 
        
            RuleFor(x =>x.WalletId)
                .NotEmpty().WithMessage("Wallet ID is required.")
                .GreaterThan(0).WithMessage("Wallet ID must be greater than 0.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.")
                .GreaterThan(0).WithMessage("User ID must be greater than 0.");


        }
    }
}
