using E_wallet.Application.Dtos.Request;
using E_wallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Mappers
{
    public class LimitMapper
    {
        public static Limit ToEntity(LimitRequest dto)
        {
            return new Limit
            {
                Value = dto.amount,
                Scope = dto.scope,
                Type = dto.type
            };
        } 
    }
}
