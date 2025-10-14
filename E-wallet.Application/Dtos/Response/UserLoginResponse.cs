using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Response
{
    public class UserLoginResponse
    {
        public int Id { get; set; }
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
    }
}
