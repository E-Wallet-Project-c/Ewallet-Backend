using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Response
{
    public class UserBankAccountResponse
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public double Balance { get; set; }
        public int WalletId { get; set; }

    }
}
