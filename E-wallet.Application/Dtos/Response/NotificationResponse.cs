using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Response
{
    public class NotificationResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string? Event { get; set; }

        public string? Type { get; set; }

        public bool? IsRead { get; set; }

        public string? Content { get; set; }

        public string? createdAt { get; set; }


    }
}
