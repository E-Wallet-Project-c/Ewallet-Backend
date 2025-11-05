using E_wallet.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Request
{
    public class LimitRequest
    {
        
        public double amount { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LimitScope scope { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]

        public LimitType type { get; set; } 

    }
}
