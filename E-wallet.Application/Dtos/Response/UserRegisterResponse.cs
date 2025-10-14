using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Request
{
    public class UserRegisterResponse
    {
        public int? Id { get; set; } = null!;
        public string? FullName { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public DateTime? CreatedAt { get; set; } = null!;
        public DateTime? UpdatedAt { get; set; } = null!;
        public bool Success { get; set; } = true;
        public string? Message { get; set; }

       


    }
}
