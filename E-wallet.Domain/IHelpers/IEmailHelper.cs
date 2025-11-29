using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Domain.IHelpers
{
    public interface IEmailHelper
    {
        Task SendOtpEmailAsync(string email, string otp, string UserName);
    }
}
