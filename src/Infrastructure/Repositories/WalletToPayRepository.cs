using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WalletToPayRepository : IWalletToPayRepository
    {
        private readonly FinanceOrganizationContext _context;

        public WalletToPayRepository(FinanceOrganizationContext context)
        {
            _context = context;
        }

        public async Task<IList<BillToPay>> GetBillToPayByFixedInvoiceId(int fixedInvoiceId)
        {
            try
            {
                var result = await _context.BillToPay
                        .AsNoTracking()
                        .Where(pay => pay.IdFixedInvoice == fixedInvoiceId)
                        .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Save(IList<BillToPay> billsToPay)
        {
            try
            {
                _context.AddRange(billsToPay);
                var result = await _context.SaveChangesAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}