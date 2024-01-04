using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IFixedInvoiceRepository
    {
        Task<int> Edit(FixedInvoice fixedInvoice);
        Task<IList<FixedInvoice>> GetAll();
        Task<IList<FixedInvoice>> GetAutomationParticipantsOnly(string registrationType);
        Task<FixedInvoice?> GetById(int iD);
        Task<FixedInvoice?> GetFixedInvoiceByName(string? name);
        Task<IList<FixedInvoice>> GetOnlyOldRecordsAndParticipants(int daysLater, string registrationType);
        Task<int> Save(FixedInvoice fixedInvoice);
    }
}