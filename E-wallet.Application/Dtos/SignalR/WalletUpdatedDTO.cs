using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.SignalR
{
    public class WalletUpdatedDTO
    {

        public int UserId { get; set; }
        public int WalletId { get; set; }
        public double Balance { get; set; }
    }
}
