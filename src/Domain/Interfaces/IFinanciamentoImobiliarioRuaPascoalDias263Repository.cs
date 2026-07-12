using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IFinanciamentoImobiliarioRuaPascoalDias263Repository
    {
        Task<IList<FinanciamentoImobiliarioRuaPascoalDias263>> GetAllInstallments();
    }
}
