using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Request
{
    public class NewPasswordrequest
    {
        public int id{ get; set; }
        public string newPassword { get; set; }
        public string confirmPassword { get; set; }
    }
}
