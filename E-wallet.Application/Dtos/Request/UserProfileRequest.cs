using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Request
{
    public class UserProfileRequest
    {
        public DateOnly? DateOfBirth { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
    }
}
