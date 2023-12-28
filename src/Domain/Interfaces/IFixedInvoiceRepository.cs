using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IFixedInvoiceRepository
    {
        Task<IList<FixedInvoice>> GetByAll();
        Task<FixedInvoice?> GetById(int iD);
        Task<bool?> GetFixedInvoiceByName(string? name);
        Task<int> Save(FixedInvoice fixedInvoice);
    }
}