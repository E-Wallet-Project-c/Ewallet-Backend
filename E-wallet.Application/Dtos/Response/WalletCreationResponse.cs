using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Response
{
     public class WalletCreationResponse
    {
        public int userId { get; set; }
        public int WalletId { get; set; }
        public string? Message { get; set; }
    }

}
