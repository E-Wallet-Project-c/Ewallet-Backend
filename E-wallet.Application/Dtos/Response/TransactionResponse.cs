using E_wallet.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Response
{
    public class TransactionResponse
    {

        public int Id { get; set; }
        public double Amount { get; set; }
        public TransactionType Type { get; set; }
        public TransactionAction Action { get; set; }
        public DateTime? CreatedAt { get; set; }

        public int? TransferId { get; set; }
        public int? SenderWalletId { get; set; }
        public int? ReceiverWalletId { get; set; }
    }
}
