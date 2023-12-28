using Domain.Interfaces;

namespace Application.Feature.FixedInvoice.CreateFixedInvoice
{
    public class CreateFixedInvoiceValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(
            CreateFixedInvoiceInput input,
            IFixedInvoiceRepository fixedInvoiceRepository)
        {
            return await CreateValidateBaseInput(input, fixedInvoiceRepository);
        }

        public static async Task<Dictionary<string, string>> CreateValidateBaseInput(CreateFixedInvoiceInput input,
            IFixedInvoiceRepository fixedInvoiceRepository)
        {
            Dictionary<string, string> validatorBase = new();

            var result = await fixedInvoiceRepository.GetFixedInvoiceByName(input.Name);

            if (result.Value)
            {
                validatorBase.Add("[32]", $"Já existe uma conta a pagar cadastada com este nome {input.Name} você deve editar para prosseguir.");
            }

            return validatorBase;
        }
    }
}