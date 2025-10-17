using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Response
{
     public class WalletResponse
    {
        public int userId { get; set; }
        public int WalletId { get; set; }
        public string? Message { get; set; }=null!;
        public string? Currency { get; set; }=null!;
        public bool? IsDeleted { get; set; } = null!;
        public bool? IsActive { get; set; }=null!;
        public DateTime? UpdatedAt { get; set; } = null!;
        public bool IsDefaultWallet { get; set; } = true;

        public DateTime? CreatedAt { get; set; }= null!;

    }

}
