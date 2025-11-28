using E_wallet.Application.Dtos.Response;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Entities;
using Microsoft.AspNetCore.Server.IISIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Mappers
{
    public static class WalletMapper
    {
        public static WalletResponse ToResponse(Wallet wallet) 
        {
            return new WalletResponse
            {
                userId = wallet.UserId,
                WalletId = wallet.Id,
                Currency =  wallet.Currency,
                Balance = wallet.Balance,
                WalletName= wallet.IsDefaultWallet ? "Default Wallet" : "Secondary Wallet",
                 IsActive= wallet.IsActive,
                  IsDefault= wallet.IsDefaultWallet,
            };
        }


        public static  List< WalletResponse> ToListResponse(List<Wallet> wallet) 
        {
            List < WalletResponse > wallets = new List<WalletResponse>();
            foreach (Wallet item in wallet)
            {
                wallets.Add(new WalletResponse
                {
                    userId = item.UserId,
                    WalletId = item.Id,
                } );
            }
            return wallets;
        }


        public static Wallet ToEntity(int _UserId,bool IsDefault )
        {
            return new Wallet
            {
                UserId = _UserId,
                Currency = "JD",
                IsActive = true,
                IsDeleted = false,
                IsDefaultWallet = IsDefault,
                CreatedAt = DateTime.Now,
                CreatedBy = null
            };
        }
     
    }
}
