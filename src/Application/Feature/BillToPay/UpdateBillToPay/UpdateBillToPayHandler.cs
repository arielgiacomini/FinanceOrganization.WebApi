using Domain.Interfaces;

namespace Application.Feature.BillToPay.UpdateBillToPay
{
    public class UpdateBillToPayHandler : IUpdateBillToPayHandler
    {
        private readonly IWalletToPayRepository _walletToPayRepository;

        public UpdateBillToPayHandler(IWalletToPayRepository walletToPayRepository)
        {
            _walletToPayRepository = walletToPayRepository;
        }

        public async Task<UpdateBillToPayOutput> Handle(UpdateBillToPayInput input, CancellationToken cancellationToken)
        {
            var billToPay = MapInputToDomain(input);

            var result = await _walletToPayRepository.Edit(billToPay);

            var output = new UpdateBillToPayOutput
            {
                Output = OutputBaseDetails.Success($"[{result}] - Cadastro realizado com sucesso.", new object())
            };

            return output;
        }

        private static Domain.Entities.BillToPay MapInputToDomain(UpdateBillToPayInput updateBillToPayInput)
        {
            return new Domain.Entities.BillToPay()
            {
                Id = updateBillToPayInput.Id,
                IdFixedInvoice = updateBillToPayInput.IdFixedInvoice,
                Account = updateBillToPayInput.Account,
                Name = updateBillToPayInput.Name,
                Category = updateBillToPayInput.Category,
                Value = updateBillToPayInput.Value,
                DueDate = updateBillToPayInput.DueDate,
                YearMonth = updateBillToPayInput.YearMonth,
                Frequence = updateBillToPayInput.Frequence,
                PayDay = updateBillToPayInput.PayDay,
                HasPay = updateBillToPayInput.HasPay
            };
        }
    }
}