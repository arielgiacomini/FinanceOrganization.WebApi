using Domain.Interfaces;
using Domain.Utils;

namespace Application.Feature.BillToPay.DeleteBillToPay
{
    public static class DeleteBillToPayValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(
            DeleteBillToPayInput input,
            IBillToPayRegistrationRepository billToPayRegistrationRepository,
            IBillToPayRepository billToPayRepository)
        {
            return await DeleteValidateBaseInput(input, billToPayRegistrationRepository, billToPayRepository);
        }

        public static async Task<Dictionary<string, string>> DeleteValidateBaseInput(DeleteBillToPayInput input,
            IBillToPayRegistrationRepository billToPayRegistrationRepository, IBillToPayRepository billToPayRepository)
        {
            Dictionary<string, string> validatorBase = new();
            int contador = 0;

            if (input.IdBillToPayRegistrations != null)
            {
                foreach (var idBillToPayRegistration in input.IdBillToPayRegistrations)
                {
                    var billToPayRegistration = await billToPayRegistrationRepository.GetById(idBillToPayRegistration);

                    if (billToPayRegistration == null)
                    {
                        validatorBase.Add("[17]", $"Não foi encontrado o IdBillToPayRegistration informado: {idBillToPayRegistration}");
                    }

                    if (billToPayRegistration != null)
                    {
                        var billToPays = await billToPayRepository.GetBillToPayByBillToPayRegistrationId(idBillToPayRegistration);

                        if (billToPays != null && billToPays!.Count > 0)
                        {
                            foreach (var billToPay in billToPays)
                            {
                                if (billToPay.HasPay && input.JustUnpaid)
                                {
                                    validatorBase.Add("[19]", $"Quando escolhida a opção " +
                                        $"[JustUnpaid] no input apenas itens não pagos devem ser deletados: {billToPay}");
                                }

                                var dateCheck = DateServiceUtils.GetDateTimeByYearMonthBrazilian(billToPay.YearMonth);
                                var nowCheck = DateServiceUtils.GetYearMonthPortugueseByDateTime(DateTime.Now);
                                var dateNowCheck = DateServiceUtils.GetDateTimeByYearMonthBrazilian(nowCheck);

                                if (billToPay.HasPay && !input.JustUnpaid && dateCheck < dateNowCheck)
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
                foreach (var idBillToPay in input.Id)
                {
                    contador++;
                    var billToPay = await billToPayRepository.GetBillToPayById(idBillToPay);

                    if (billToPay == null)
                    {
                        validatorBase.Add($"[20-{contador}]", $"Não foi encontrado nenhum BillToPay para o Id informado: [{idBillToPay}]");
                    }

                    if (billToPay != null)
                    {
                        var billToPayRegistration = await billToPayRegistrationRepository.GetById(billToPay.IdBillToPayRegistration);

                        if (billToPayRegistration == null)
                        {
                            validatorBase.Add($"[21-{contador}]", $"Não foi encontrado nenhum BillToPayRegistration para o Id informado: [{billToPay.IdBillToPayRegistration}]");
                        }

                        if (billToPay.HasPay && input.JustUnpaid)
                        {
                            validatorBase.Add($"[22-{contador}]", $"Quando escolhida a opção " +
                                $"[JustUnpaid] no input apenas itens não pagos devem ser deletados: {billToPay}");
                        }

                        var dateCheck = DateServiceUtils.GetDateTimeByYearMonthBrazilian(billToPay.YearMonth);
                        var nowCheck = DateServiceUtils.GetYearMonthPortugueseByDateTime(DateTime.Now);
                        var dateNowCheck = DateServiceUtils.GetDateTimeByYearMonthBrazilian(nowCheck);

                        if (billToPay.HasPay && !input.JustUnpaid && dateCheck < dateNowCheck)
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