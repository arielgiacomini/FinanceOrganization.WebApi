using Domain.Entities;

namespace Domain.Interfaces;

public interface ICashReceivableRegistrationRepository
{
    Task<int> Edit(CashReceivableRegistration registration);
    Task<IList<CashReceivableRegistration>> GetOnlyOldRecordsAndParticipants(int daysLater, string registrationType);
    Task<int> Save(CashReceivableRegistration cashReceivable);
}