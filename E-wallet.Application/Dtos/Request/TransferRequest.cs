using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Request
{
    public class TransferRequest
    {
        public int SenderWalletId { get; set; }
        public int ReceiverWalletId { get; set; }
        public int Amount { get; set; }
        public double TransferFee { get; set; }
    }
}
