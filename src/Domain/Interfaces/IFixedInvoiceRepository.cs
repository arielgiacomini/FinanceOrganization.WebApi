using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IFixedInvoiceRepository
    {
        Task<IList<FixedInvoice>> GetByAll();
    }
}