using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Response
{
     public class WalletResponse
    {
        public int userId { get; set; }
        public int WalletId { get; set; }

        public string WalletName { get; set; }= string.Empty;

        public double  Balance { get; set; }
         
        public bool? IsActive { get; set; }

        public bool IsDefault { get; set; }

        public string Currency { get; set; }= string.Empty;
    }

}
