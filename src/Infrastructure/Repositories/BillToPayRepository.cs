using Domain.Entities;
using Domain.Entities.Extern;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Repositories
{
    public class BillToPayRepository : IBillToPayRepository
    {
        private readonly FinanceOrganizationContext _context;
        private readonly ILogger _logger;

        public BillToPayRepository(ILogger logger,
            FinanceOrganizationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IList<BillToPay>> GetBillToPayByFixedInvoiceId(int fixedInvoiceId)
        {
            try
            {
                var result = await _context.BillToPay!
                    .AsNoTracking()
                    .Where(pay => pay.IdFixedInvoice == fixedInvoiceId)
                    .OrderBy(pay => pay.DueDate)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Exception exception = new(ex.Message);
                throw exception;
            }
        }

        public async Task<IList<BillToPay>> GetBillToPayByYearMonthAndCategoryAndRegistrationType(string yearMonth, string category, string registationType)
        {
            try
            {
                var result = await _context.BillToPay!
                    .AsNoTracking()
                    .Where(pay => pay.Category == category
                        && pay.RegistrationType == registationType && pay.YearMonth == yearMonth)
                    .OrderBy(pay => pay.DueDate)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Exception exception = new(ex.Message);
                throw exception;
            }
        }

        public async Task<BillToPay?> GetBillToPayById(Guid id)
        {
            var billToPayOrigin = await _context.BillToPay!
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);

            return billToPayOrigin;
        }

        public async Task<BillToPay?> GetBillToPayByNameDueDateAndFrequence(string name, string yearMonth, string frequence)
        {
            var billToPay = await _context.BillToPay!
                .AsNoTracking()
                .FirstOrDefaultAsync(bill => bill.Name == name && bill.YearMonth == yearMonth && bill.Frequence == frequence);

            return billToPay;
        }

        public async Task<BillToPay?> GetByYearMonthCategoryAndRegistrationType(
            string yearMonth, string category, string registrationType)
        {
            var billToPay = await _context.BillToPay!
                .AsNoTracking()
                .FirstOrDefaultAsync(bill =>
                   bill.YearMonth == yearMonth
                && bill.Category == category
                && bill.RegistrationType == registrationType);

            return billToPay;
        }

        public async Task<IList<BillToPay>?> GetNotPaidYetByYearMonthAndAccount(string yearMonth, string account)
        {
            var creditCardBill = await _context.BillToPay!
                .AsNoTracking()
                .Where(creditCard => creditCard.Account == account
                    && creditCard.YearMonth == yearMonth
                    && !creditCard.HasPay
                    && string.IsNullOrWhiteSpace(creditCard.PayDay))
                .ToListAsync();

            return creditCardBill;
        }

        public async Task<IList<BillToPay>?> GetBillToPayByYearMonth(string yearMonth)
        {
            var billsToPay = await _context.BillToPay!
                .AsNoTracking()
                .Where(x => x.YearMonth == yearMonth)
                .ToListAsync();

            return billsToPay;
        }

        public async Task<int> SaveRange(IList<BillToPay> billsToPay)
        {
            int contador = 0;

            foreach (var item in billsToPay)
            {
                try
                {
                    _context.Add(item);

                    _context.SaveChanges();

                    contador++;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "{message}, item: {@item}", ex.Message, item);
                    throw;
                }
            }

            return await Task.FromResult(contador);
        }

        public async Task<int> Edit(BillToPay billToPay)
        {
            _context.ChangeTracker.Clear();

            _context.BillToPay!.Update(billToPay);

            var result = _context.SaveChanges();

            return await Task.FromResult(result);
        }

        public async Task<int> EditRange(IList<BillToPay> billToPays)
        {
            _context.ChangeTracker.Clear();

            _context.BillToPay!.UpdateRange(billToPays);

            var result = _context.SaveChanges();

            return await Task.FromResult(result);
        }

        public async Task<int> DeleteRange(IList<BillToPay> billToPays)
        {
            _context.ChangeTracker.Clear();

            _context.BillToPay!.RemoveRange(billToPays);

            var result = _context.SaveChanges();

            return await Task.FromResult(result);
        }

        public async Task<int> Delete(BillToPay billToPay)
        {
            _context.ChangeTracker.Clear();

            _ = _context.BillToPay!.Remove(billToPay);

            var result = _context.SaveChanges();

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Trás os resultos de contas a pagar dos últimos meses com base nos parâmetros informados
        /// </summary>
        /// <param name="initialDate"></param>
        /// <param name="finallyDate"></param>
        /// <param name="qtyMonth"></param>
        /// <returns></returns>
        public async Task<IList<MonthlyAverageAnalysis>> GetMonthlyAverageAnalysesAsync(DateTime initialDate, DateTime finallyDate, int qtyMonth)
        {
            try
            {
                var result = _context
                    .MonthlyAverageAnalysis!
                    .FromSqlInterpolated($"EXECUTE [dbo].[STP_CONTA_PAGAR_MEDIAS_MENSAIS] @DATA_INICIAL = {initialDate}, @DATA_FINAL = {finallyDate}, @QUANTIDADE_MESES_ANALISE_MEDIA = {qtyMonth}")
                    .ToArray();

                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}