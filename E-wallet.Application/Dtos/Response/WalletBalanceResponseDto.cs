using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Response
{
    public class WalletBalanceResponseDto
    {
        public int WalletId { get; set; }
        public double Balance { get; set; }
        public string Currency { get; set; } = null!;
        public DateTime LastUpdated { get; set; }
    }
}
