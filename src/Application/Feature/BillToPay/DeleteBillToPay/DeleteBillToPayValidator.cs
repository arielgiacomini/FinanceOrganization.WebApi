using Domain.Interfaces;
using Domain.Utils;

namespace Application.Feature.BillToPay.DeleteBillToPay
{
    public static class DeleteBillToPayValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(
            DeleteBillToPayInput input,
            IBillToPayRegistrationRepository fixedInvoiceRepository,
            IBillToPayRepository billToPayRepository)
        {
            return await DeleteValidateBaseInput(input, fixedInvoiceRepository, billToPayRepository);
        }

        public static async Task<Dictionary<string, string>> DeleteValidateBaseInput(DeleteBillToPayInput input,
            IBillToPayRegistrationRepository fixedInvoiceRepository, IBillToPayRepository billToPayRepository)
        {
            Dictionary<string, string> validatorBase = new();
            int contador = 0;

            if (input.IdFixedInvoices != null)
            {
                foreach (var idFixedInvoice in input.IdFixedInvoices)
                {
                    var fixedInvoice = await fixedInvoiceRepository.GetById(idFixedInvoice);

                    if (fixedInvoice == null)
                    {
                        validatorBase.Add("[17]", $"Não foi encontrado o IdFixedInvoice informado: {idFixedInvoice}");
                    }

                    if (fixedInvoice != null)
                    {
                        var billToPays = await billToPayRepository.GetBillToPayByFixedInvoiceId(idFixedInvoice);

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
                        var fixedInvoice = await fixedInvoiceRepository.GetById(billToPay.IdFixedInvoice);

                        if (fixedInvoice == null)
                        {
                            validatorBase.Add($"[21-{contador}]", $"Não foi encontrado nenhum FixedInvoice para o Id informado: [{billToPay.IdFixedInvoice}]");
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