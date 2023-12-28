﻿using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Feature.BillToPay.EditBillToPay
{
    public class EditBillToPayHandler : IEditBillToPayHandler
    {
        private readonly ILogger<EditBillToPayHandler> _logger;
        private readonly IWalletToPayRepository _walletToPayRepository;

        public EditBillToPayHandler(ILogger<EditBillToPayHandler> logger, IWalletToPayRepository walletToPayRepository)
        {
            _logger = logger;
            _walletToPayRepository = walletToPayRepository;
        }

        public async Task<EditBillToPayOutput> Handle(EditBillToPayInput input, CancellationToken cancellationToken)
        {
            var validate = await EditBillToPayValidator.ValidateInput(input, _walletToPayRepository);

            if (validate.Any())
            {
                var outputValidator = new EditBillToPayOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var billToPay = MapInputToDomain(input);

            var result = await _walletToPayRepository.Edit(billToPay);

            var output = new EditBillToPayOutput
            {
                Output = OutputBaseDetails.Success($"[{result}] - Cadastro realizado com sucesso.", new object())
            };

            return output;
        }

        private static Domain.Entities.BillToPay MapInputToDomain(EditBillToPayInput updateBillToPayInput)
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
                HasPay = updateBillToPayInput.HasPay,
                LastChangeDate = updateBillToPayInput.LastChangeDate
            };
        }
    }
}