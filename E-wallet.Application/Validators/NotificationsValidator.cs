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
    public class NotificationsValidator : AbstractValidator<NotificationRequest>
    {
        public NotificationsValidator (){
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Type is required.")
                .GreaterThan(0).WithMessage("The ID must be greater than 0");

        

            RuleFor(x => x.Event)
                .NotEmpty().WithMessage("Event is required.")
                .Must(type => Enum.TryParse<NotificationEvents>(type,false,out NotificationEvents result))
                .WithMessage("Invalid notification Events.");


            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(250).WithMessage("The name must not exceed 50 character")
                .MinimumLength(5).WithMessage("The name should be at least 5 character") ;



        }

    }
}
