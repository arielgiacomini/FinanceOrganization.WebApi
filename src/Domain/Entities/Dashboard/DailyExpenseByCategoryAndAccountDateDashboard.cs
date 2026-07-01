namespace Domain.Entities.Dashboard
{
    public class DailyExpenseByCategoryAndAccountDateDashboard
    {
        public string? Type { get; set; }
        public string? Account { get; set; }
        public string? Category { get; set; }
        public DateTime? Date { get; set; }
        public int? Day { get; set; }
        public string? Month { get; set; }
        public string? DayWeek { get; set; }
        public bool? Weekend { get; set; }
        public bool? Holiday { get; set; }
        public string? TaxCountry { get; set; }
        public string? MonthYear { get; set; }
        public int? Quantity { get; set; }
        public decimal? Value { get; set; }
        public decimal? ManipulatedValue { get; set; }
        public bool? HasPay { get; set; }
    }

    public class QuerySqlDailyExpenseByCategoryAndAccountDateDashboard
    {
        private readonly string _years;
        private readonly string _months;
        private readonly string _category;

        public QuerySqlDailyExpenseByCategoryAndAccountDateDashboard(string? years, string? months, string? category)
        {
            _years = string.IsNullOrEmpty(years) ? string.Concat(DateTime.Now.Year, ",", DateTime.Now.AddYears(1).Year) : years;
            _months = string.IsNullOrEmpty(months) ? "1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12" : months;
            _category = category ?? "Alimentação:Café da Manhã";
        }

        /// <summary>
        /// Script SQL para buscar os dados do Dashboard Contas a Pagar/Receber por categoria e conta
        /// </summary>
        public FormattableString Sql => $@"
                DECLARE @ANOS_DEFAULT VARCHAR(10) = (SELECT CAST(YEAR(GETDATE()) AS VARCHAR(4)) AS MesesDefault);
				DECLARE @ANOS VARCHAR(MAX) = {_years}
				DECLARE @MESES VARCHAR(MAX) = {_months}
				DECLARE @CATEGORIA VARCHAR(MAX) = {_category}
				
				;WITH ContaPagar (Data, PaisFiscal, MesAno, Dia, Mes, DiaSemana, FimDeSemana, Feriado, Categoria, Conta, Quantidade, Valor, ValorManipulado, Pago) 
				AS (
					SELECT 
							DimData.Data, 
							DSC_PAIS_FISCAL AS PaisFiscal, 
							IND_MES_ANO,
							DadosDatas.Dia,
							DadosDatas.NomeMes,
							DadosDatas.NomeDiaSemana,
							DadosDatas.FimDeSemana,
							DadosDatas.EhFeriado,
							DSC_CATEGORIA, 
							DSC_CONTA, 
							SUM(CASE WHEN IND_PAGO = 1 AND VAL_VALOR <= 0 THEN 0 ELSE 1 END) AS Quantidade, 
							SUM(VAL_VALOR) AS Valor, 
							CASE WHEN IND_PAGO = 0 THEN SUM(VAL_VALOR) ELSE 0 END AS ValorManipulado, 
							IND_PAGO
						FROM CONTA_PAGAR
							INNER JOIN DimData ON DimData.MesAno = CONTA_PAGAR.IND_MES_ANO AND Dia = 1
							LEFT JOIN (
										SELECT * FROM DimData
									  ) AS DadosDatas 
											ON CAST(ISNULL(ISNULL(ISNULL(CONTA_PAGAR.DAT_COMPRA, CONTA_PAGAR.DAT_VENCIMENTO), DAT_PAGAMENTO), DAT_CRIACAO_REGISTRO) AS DATE) = DadosDatas.Data
							WHERE 1=1
								AND DimData.Ano IN (SELECT TRY_CAST(TRIM(value) AS INT) FROM STRING_SPLIT(@ANOS, ',') WHERE TRY_CAST(TRIM(value) AS INT) IS NOT NULL)
								AND DimData.Mes IN (SELECT TRY_CAST(TRIM(value) AS INT) FROM STRING_SPLIT(@MESES, ',') WHERE TRY_CAST(TRIM(value) AS INT) IS NOT NULL)
								AND DSC_CATEGORIA IN (@CATEGORIA)
					GROUP BY DimData.Data, DSC_PAIS_FISCAL, IND_MES_ANO, IND_PAGO, DSC_CATEGORIA, DSC_CONTA, DadosDatas.Dia, DadosDatas.NomeMes, DadosDatas.NomeDiaSemana, DadosDatas.FimDeSemana, DadosDatas.EhFeriado
				)
					SELECT 
						    '1 - Gastos por Dia/Mes/Ano/Categoria' AS Type,
							ContaPagar.Conta                       AS Account,
							ContaPagar.Categoria                   AS Category,
							ContaPagar.Data                        AS Date,
							ContaPagar.Dia                         AS Day,
							ContaPagar.Mes                         AS Month,
							ContaPagar.DiaSemana                   AS DayWeek,
							ContaPagar.FimDeSemana                 AS Weekend,
							ContaPagar.Feriado                     AS Holiday,
							ContaPagar.PaisFiscal                  AS TaxCountry,
							ContaPagar.MesAno                      AS MonthYear,
							ContaPagar.Quantidade                  AS Quantity, 
							ContaPagar.Valor                       AS Value,
							ContaPagar.ValorManipulado             AS ManipulatedValue,
							ContaPagar.Pago			               AS HasPay
					FROM ContaPagar
						WHERE 1=1
					ORDER BY Dia";
    }
}