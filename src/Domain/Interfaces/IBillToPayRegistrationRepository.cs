using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBillToPayRegistrationRepository
    {
        Task<int> Delete(BillToPayRegistration billToPayRegistration);
        Task<int> DeleteRange(IList<BillToPayRegistration> billToPayRegistrations);
        Task<int> Edit(BillToPayRegistration billToPayRegistration);
        Task<IList<BillToPayRegistration>> GetAll();
        Task<IList<BillToPayRegistration>> GetAutomationParticipantsOnly(string registrationType);
        Task<BillToPayRegistration?> GetById(int iD);
        Task<BillToPayRegistration?> GetBillToPayRegistrationByName(string? name);
        Task<IList<BillToPayRegistration>> GetOnlyOldRecordsAndParticipants(int daysLater, string registrationType);
        Task<int> Save(BillToPayRegistration billToPayRegistration);
        Task<IList<BillToPayRegistration>> GetBillToPayNotRegistrationPrincipal();
    }
}