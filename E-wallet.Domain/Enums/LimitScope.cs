using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_wallet.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum LimitScope
    {
        DAILY,
        WEEKLY,
        MONTHLY
    }
}
