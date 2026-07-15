namespace Domain.Entities
{
    public class FinanciamentoImobiliarioRuaPascoalDias263
    {
        /// <summary>
        /// Installment - representa o campo Parcela da Tabela
        /// </summary>
        public int? Installment { get; set; }
        /// <summary>
        /// Serie - Representa o campo Serie da tabela.
        /// </summary>
        public string? Serie { get; set; }
        /// <summary>
        /// Currency Type - Representa o campo tipo da moeda "R$"
        /// </summary>
        public string? CurrencyType { get; set; }
        /// <summary>
        /// Due date - Representa o campo DataVencimento da tabela.
        /// </summary>
        public DateTime? DueDate { get; set; }
        /// <summary>
        /// Amortization System - Represeta o campo Sitema Amort da tabela.
        /// </summary>
        public string? AmortizationSystem { get; set; }
        /// <summary>
        /// EffectiveAnnualRateAA - Representa o campo TaxaEfetivaAA da tabela
        /// </summary>
        public decimal? EffectiveAnnualRateAA { get; set; }
        /// <summary>
        /// EffectiveAnnualRateAM - Represeta o campo TaxaEfetivaAM da tabela
        /// </summary>
        public decimal? EffectiveAnnualRateAM { get; set; }
        /// <summary>
        /// NominalRateAA - Representa o campo TaxaNominalAA da tabela
        /// </summary>
        public decimal? NominalRateAA { get; set; }
        /// <summary>
        /// NominalRateAM - Representa o campo TaxaNominalAM
        /// </summary>
        public decimal? NominalRateAM { get; set; }
        /// <summary>
        /// Amortization - Representa o campo Amortizacao da tabela
        /// </summary>
        public decimal? Amortization { get; set; }
        /// <summary>
        /// Fees - Represeta o campo Juros da tabela
        /// </summary>
        public decimal? Fees { get; set; }
        /// <summary>
        /// InstallmentAdjustmentIndex - Representa o campo IndiceCorrecaoPArcela da tabela
        /// </summary>
        public decimal? InstallmentAdjustmentIndex { get; set; }
        /// <summary>
        /// SafeMIP - Representa o campo SeguroMIP da tabela
        /// </summary>
        public decimal? SafeMIP { get; set; }
        /// <summary>
        /// SafeDFI - Representa o campo SeguroDFI da tabela
        /// </summary>
        public decimal? SafeDFI { get; set; }
        /// <summary>
        /// TCA - Represeta o campo TCA da tabela
        /// </summary>
        public decimal? TCA { get; set; }
        /// <summary>
        /// More - Representa o campo Multa da tabela
        /// </summary>
        public decimal? More { get; set; }
        /// <summary>
        /// Mora - Representa o campo Mora da tabela
        /// </summary>
        public decimal? Mora { get; set; }
        /// <summary>
        /// FinancialAdjustment - Representa o campo AjusteFinanceiro da tabela
        /// </summary>
        public decimal? FinancialAdjustment { get; set; }
        /// <summary>
        /// MonthlyFGTS - Representa o campo FGTSMensal da tabela
        /// </summary>
        public decimal? MonthlyFGTS { get; set; }
        /// <summary>
        /// BalanceAdjustmentIndex - Representa o campo IndiceCorrecaoSaldo da tabela
        /// </summary>
        public decimal? BalanceAdjustmentIndex { get; set; }
        /// <summary>
        /// OutstandingBalance - Representa o campo SaldoDevedor da tabela
        /// </summary>
        public decimal? OutstandingBalance { get; set; }
        /// <summary>
        /// ResInsurance - Representa o campo SeguroRES da tabela
        /// </summary>
        public decimal? ResInsurance { get; set; }
        /// <summary>
        /// Representa o campo Situacao da tabela
        /// </summary>
        public string? Situation { get; set; }
        /// <summary>
        /// TotalInstallmentAmount - Representa o campo ValorTotalParcela da tabela
        /// </summary>
        public decimal? TotalInstallmentAmount { get; set; }
    }
}