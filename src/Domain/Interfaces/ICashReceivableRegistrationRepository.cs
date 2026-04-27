using Domain.Entities;

namespace Domain.Interfaces;

public interface ICashReceivableRegistrationRepository
{
    Task<int> Delete(CashReceivableRegistration cashReceivableRegistration);
    Task<int> DeleteRange(IList<CashReceivableRegistration> cashReceivableRegistrations);
    Task<int> Disable(int id);
    Task<int> Edit(CashReceivableRegistration registration);
    Task<CashReceivableRegistration?> GetById(int iD);
    Task<IList<CashReceivableRegistration>> GetOnlyOldRecordsAndParticipants(int daysLater, string registrationType);
    Task<int> Save(CashReceivableRegistration cashReceivable);
}