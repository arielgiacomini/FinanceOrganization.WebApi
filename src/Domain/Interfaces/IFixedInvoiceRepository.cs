using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IFixedInvoiceRepository
    {
        Task<IList<FixedInvoice>> GetByAll();
        Task<int> Save(FixedInvoice fixedInvoice);
    }
}