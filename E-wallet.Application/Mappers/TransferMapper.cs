using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Mappers
{
    public class TransferMapper
    {
        public static Transfer ToTransferEntity(TransferRequest dto)
        {
            return new Transfer
            {
              SenderWalletId=dto.SenderWalletId,
              ReciverWalletId=dto.ReceiverWalletId,
              Amount=dto.Amount,
              Fee=dto.TransferFee,
              IsActive=true,
              CreatedAt=DateTime.Now

            };
        }


        public static TransferResponse ToTransferResponse(int transferId)
        {
            return new TransferResponse
            {
              Id= transferId,
              StatusTransfer=true,

            };
        }


    }
}
