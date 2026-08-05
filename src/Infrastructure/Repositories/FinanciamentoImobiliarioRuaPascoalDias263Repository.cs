using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Repositories
{
    public class FinanciamentoImobiliarioRuaPascoalDias263Repository : IFinanciamentoImobiliarioRuaPascoalDias263Repository //Criar Interface
    {
        private readonly FinanceOrganizationContext _context; //Direito a acessar o nosso banco -> DBContext da aplicação
        private readonly ILogger _looger; //vai guardar o log de erro

        //Construtor
        public FinanciamentoImobiliarioRuaPascoalDias263Repository(
            ILogger logger, FinanceOrganizationContext context)
        {
            _context = context;
            _looger = logger;
        }

        public async Task<IList<FinanciamentoImobiliarioRuaPascoalDias263>> GetAllInstallments() //Vai buscar e devolver uma lista
        {
            try
            {
                //variavel que vai armazezar a busca; 
                //_context.Accounts acessando o DBSet
                //ToListAsync() Vai executar a consulta e devolver o resultado em lista
                var installments = await _context.FinanciamentoImobiliarioRuaPascoalDias263.ToListAsync();

                return installments;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
