using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.SignalR
{
    public class MoneyReceivedDTO
    {
        public int ReceiverUserId { get; set; }
        public int ReceiverWalletId { get; set; }
        public int SenderUserId { get; set; }
        public double Amount { get; set; }
        public int TransferId { get; set; }
    }
}
