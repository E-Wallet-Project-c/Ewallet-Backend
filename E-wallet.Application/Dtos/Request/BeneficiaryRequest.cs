using E_wallet.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Request
{
    public class BeneficiaryRequest
    {
        public int WalletId { get; set; }

        public int BeneficiaryWalletId { get; set; }
        public string NickName { get; set; }
        public BeneficiaryPurpose Purpos { get; set; }

    }
}
