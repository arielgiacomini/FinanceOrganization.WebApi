﻿using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BillToPayRepository : IBillToPayRepository
    {
        private readonly FinanceOrganizationContext _context;

        public BillToPayRepository(FinanceOrganizationContext context)
        {
            _context = context;
        }

        public async Task<IList<BillToPay>> GetBillToPayByFixedInvoiceId(int fixedInvoiceId)
        {
            try
            {
                var result = await _context.BillToPay!
                    .AsNoTracking()
                    .Where(pay => pay.IdFixedInvoice == fixedInvoiceId)
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
                && bill.RegistrationType == registrationType
                && !bill.HasPay
                && string.IsNullOrWhiteSpace(bill.PayDay));

            return billToPay;
        }

        public async Task<IList<BillToPay>?> GetBillToPayByYearMonthAndAccount(string yearMonth, string account)
        {
            var creditCardBill = await _context.BillToPay!
                .AsNoTracking()
                .Where(creditCard => creditCard.Account == account
                    && creditCard.YearMonth == yearMonth)
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
            _context.AddRange(billsToPay);

            var result = _context.SaveChanges();

            return await Task.FromResult(result);
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
    }
}