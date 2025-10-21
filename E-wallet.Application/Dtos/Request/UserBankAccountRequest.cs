using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Request
{
    public class UserBankAccountRequest
    {
        public int WalletId { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public double Balance { get; set; }
    }
}
