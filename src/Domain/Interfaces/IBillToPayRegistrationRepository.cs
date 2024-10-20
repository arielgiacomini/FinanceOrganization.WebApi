using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBillToPayRegistrationRepository
    {
        Task<int> Delete(BillToPayRegistration fixedInvoice);
        Task<int> DeleteRange(IList<BillToPayRegistration> fixedInvoices);
        Task<int> Edit(BillToPayRegistration fixedInvoice);
        Task<IList<BillToPayRegistration>> GetAll();
        Task<IList<BillToPayRegistration>> GetAutomationParticipantsOnly(string registrationType);
        Task<BillToPayRegistration?> GetById(int iD);
        Task<BillToPayRegistration?> GetFixedInvoiceByName(string? name);
        Task<IList<BillToPayRegistration>> GetOnlyOldRecordsAndParticipants(int daysLater, string registrationType);
        Task<int> Save(BillToPayRegistration fixedInvoice);
    }
}