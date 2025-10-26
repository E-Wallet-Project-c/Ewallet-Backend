using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Request
{
    public class WalletRequest
    {
        public int WalletId { get; set; } 
        public int UserId { get; set; } 
        public  bool IsDefault { get; set; }=true;
    }
}
