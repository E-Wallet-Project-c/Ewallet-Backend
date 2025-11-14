using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Response
{
    public class BeneficiaryResponse
    {
        public int Id { get; set; }
        public int BeneficiaryWalletId{ get; set; }
        public string NickName { get; set; } = null!;
    }


}
