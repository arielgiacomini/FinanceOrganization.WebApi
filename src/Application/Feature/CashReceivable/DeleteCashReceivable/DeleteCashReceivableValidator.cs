using AGS.DateServiceUtils.Service;
using Domain.Interfaces;

namespace Application.Feature.CashReceivable.DeleteCashReceivable
{
    public static class DeleteCashReceivableValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(
                DeleteCashReceivableInput input,
                ICashReceivableRegistrationRepository cashReceivableRegistrationRepository,
                ICashReceivableRepository cashReceivableRepository)
        {
            return await DeleteValidateBaseInput(input, cashReceivableRegistrationRepository, cashReceivableRepository);
        }

        public static async Task<Dictionary<string, string>> DeleteValidateBaseInput(DeleteCashReceivableInput input,
            ICashReceivableRegistrationRepository cashReceivableRegistrationRepository, ICashReceivableRepository cashReceivableRepository)
        {
            Dictionary<string, string> validatorBase = new();
            int contador = 0;

            if (input.IdCashReceivableRegistrations != null)
            {
                foreach (var idCashReceivableRegistration in input.IdCashReceivableRegistrations)
                {
                    var cashReceivableRegistration = await cashReceivableRegistrationRepository.GetById(idCashReceivableRegistration);
                    if (cashReceivableRegistration == null)
                    {
                        validatorBase.Add("[17]", $"Não foi encontrado o IdCashReceivableRegistration informado: {idCashReceivableRegistration}");
                    }

                    if (cashReceivableRegistration != null)
                    {
                        var cashReceivables = await cashReceivableRepository.GetCashReceivableRegistrationId(idCashReceivableRegistration);

                        if (cashReceivables != null && cashReceivables!.Count > 0)
                        {
                            foreach (var cashReceivable in cashReceivables)
                            {
                                if (cashReceivable.HasReceived && input.OnlyNotReceivable)
                                {
                                    validatorBase.Add("[19]", $"Quando escolhida a opção " +
                                        $"[OnlyNotReceivable] no input apenas itens não recebidos devem ser deletados: {cashReceivable}");
                                }

                                var dateCheck = DateServiceUtils.GetDateTimeByYearMonthBrazilian(cashReceivable.YearMonth);
                                var nowCheck = DateServiceUtils.GetYearMonthPortugueseByDateTime(DateTime.Now);
                                var dateNowCheck = DateServiceUtils.GetDateTimeByYearMonthBrazilian(nowCheck);

                                if (cashReceivable.HasReceived && !input.OnlyNotReceivable && dateCheck < dateNowCheck)
                                {
                                    validatorBase.Add("[23]", $"Não pode ser deletado registro de um Mês/Ano fechado.");
                                }
                            }
                        }
                    }
                }
            }

            if (input.Id != null)
            {
                foreach (var idCashReceivable in input.Id)
                {
                    contador++;
                    var billToPay = await cashReceivableRepository.GetById(idCashReceivable);

                    if (billToPay == null)
                    {
                        validatorBase.Add($"[20-{contador}]", $"Não foi encontrado nenhum CashReceivable para o Id informado: [{idCashReceivable}]");
                    }

                    if (billToPay != null)
                    {
                        var billToPayRegistration = await cashReceivableRegistrationRepository.GetById(billToPay.IdCashReceivableRegistration);

                        if (billToPayRegistration == null)
                        {
                            validatorBase.Add($"[21-{contador}]", $"Não foi encontrado nenhum CashReceivableRegistration para o Id informado: [{billToPay.IdCashReceivableRegistration}]");
                        }

                        if (billToPay.HasReceived && input.OnlyNotReceivable)
                        {
                            validatorBase.Add($"[22-{contador}]", $"Quando escolhida a opção " +
                                $"[OnlyNotReceivable] no input apenas itens não recebidos devem ser deletados: {billToPay}");
                        }

                        var dateCheck = DateServiceUtils.GetDateTimeByYearMonthBrazilian(billToPay.YearMonth);
                        var nowCheck = DateServiceUtils.GetYearMonthPortugueseByDateTime(DateTime.Now);
                        var dateNowCheck = DateServiceUtils.GetDateTimeByYearMonthBrazilian(nowCheck);

                        if (billToPay.HasReceived && !input.OnlyNotReceivable && dateCheck < dateNowCheck)
                        {
                            validatorBase.Add($"[23-{contador}]", $"Não pode ser deletado registro de um Mês/Ano fechado.");
                        }
                    }
                }
            }

            return validatorBase;
        }
    }
}