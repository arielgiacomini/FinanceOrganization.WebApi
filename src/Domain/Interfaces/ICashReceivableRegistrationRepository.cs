using Domain.Entities;

namespace Domain.Interfaces;

public interface ICashReceivableRegistrationRepository
{
    Task<int> Save(CashReceivableRegistration cashReceivable);
}