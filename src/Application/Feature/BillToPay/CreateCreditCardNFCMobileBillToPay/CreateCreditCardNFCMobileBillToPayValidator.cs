﻿using Domain.Interfaces;

namespace Application.Feature.BillToPay.CreateCreditCardNFCMobileBillToPay
{
    public class CreateCreditCardNFCMobileBillToPayValidator
    {
        private const string CARTAO_CREDITO = "Cartão de Crédito";
        private const string CARTAO_DEBITO = "Cartão de Débito";
        private const string CARTAO_VALE_ALIMENTACAO = "Cartão VA";
        private const string CARTAO_VALE_REFEICAO = "Cartão VR";

        public static async Task<Dictionary<string, string>> ValidateInput(
            CreateCreditCardNFCMobileBillToPayInput input,
            IFixedInvoiceRepository fixedInvoiceRepository,
            IBillToPayRepository billToPayRepository)
        {
            return await CreateValidateBaseInput(input, fixedInvoiceRepository, billToPayRepository);
        }

        public static async Task<Dictionary<string, string>> CreateValidateBaseInput(CreateCreditCardNFCMobileBillToPayInput input,
            IFixedInvoiceRepository fixedInvoiceRepository, IBillToPayRepository billToPayRepository)
        {
            Dictionary<string, string> validatorBase = new();

            return validatorBase;
        }

        public static bool AccountIsValidRule(string? account)
        {
            bool isAccountValidRule = true;
            switch (account)
            {
                case CARTAO_CREDITO:
                    isAccountValidRule = false;
                    break;
                case CARTAO_VALE_ALIMENTACAO:
                    isAccountValidRule = false;
                    break;
                case CARTAO_VALE_REFEICAO:
                    isAccountValidRule = false;
                    break;
                case CARTAO_DEBITO:
                    isAccountValidRule = false;
                    break;
                default:
                    break;
            }

            return isAccountValidRule;
        }
    }
}