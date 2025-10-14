using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Request
{
    public  class VerifyOtpRequest
    {
        public int UserId { get; set; }
        public string OtpCode { get; set; }
        public string Purpose { get; set; }
    }
}
