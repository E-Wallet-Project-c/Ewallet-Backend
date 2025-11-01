using E_wallet.Domain.Context;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Infrastrucure.Repositories
{
    public class TransferRepository : ITransferRepository
    {
        private readonly ApplicationDbContext _context;

        public TransferRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Transfer> AddTransfer(Transfer transfer)
        {
            var entry = await _context.Transfers.AddAsync(transfer);
           
            await _context.SaveChangesAsync();
            transfer.Id = entry.Entity.Id;
            return transfer;
        }
    }
}
