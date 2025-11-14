using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Request
{
    public class TopUpWithdrawRequest
    {
        public int UserBankAccountId { get; set; }
        public int WalletId { get; set; }
        public double Balance { get; set; }
    }
}
