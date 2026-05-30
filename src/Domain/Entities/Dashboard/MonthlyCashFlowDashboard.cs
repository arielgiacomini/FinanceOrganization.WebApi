namespace Domain.Entities.Dashboard
{
    public class MonthlyCashFlowDashboard
    {
        public DateTime? Date { get; set; }
        public string? MonthYear { get; set; }
        public int? AccountsPayableQuantity { get; set; }
        public decimal? AccountsPayableValue { get; set; }
        public int? AccountsReceivableQuantity { get; set; }
        public decimal? AccountsReceivableValue { get; set; }
    }

    public class QuerySqlMonthlyCashFlowDashboard
    {
        private readonly string _years;
        private readonly string _months;

        public QuerySqlMonthlyCashFlowDashboard(string? years, string? months)
        {
            _years = string.IsNullOrEmpty(years) ? string.Concat(DateTime.Now.Year, ",", DateTime.Now.AddYears(1).Year) : years;
            _months = string.IsNullOrEmpty(months) ? "1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12" : months;
        }

        /// <summary>
        /// Script SQL para buscar os dados do Dashboard Contas a Pagar/Receber por mês/ano, quantidade e valor.
        /// </summary>
        public FormattableString Sql => $@"
                DECLARE @ANOS_DEFAULT VARCHAR(10) = (SELECT CAST(YEAR(GETDATE()) AS VARCHAR(4)) + ',' + CAST(YEAR(DATEADD(YEAR, 1, GETDATE())) AS VARCHAR(4)) AS MesesDefault);
                DECLARE @ANOS VARCHAR(MAX) = {_years};
                DECLARE @MESES VARCHAR(MAX) = {_months};
                
                ;WITH ContaPagar (Data, MesAno, Quantidade, Valor, ValorManipulado) AS (
                	SELECT DimData.Data, IND_MES_ANO, COUNT(*) AS Quantidade, SUM(VAL_VALOR) AS Valor, SUM(VAL_VALOR) AS ValorManipulado
                		FROM CONTA_PAGAR
                			INNER JOIN DimData ON DimData.MesAno = CONTA_PAGAR.IND_MES_ANO AND Dia = 1
                			WHERE 1=1
                				AND DimData.Ano IN (SELECT TRY_CAST(TRIM(value) AS INT) FROM STRING_SPLIT(@ANOS, ',') WHERE TRY_CAST(TRIM(value) AS INT) IS NOT NULL)
                				AND DimData.Mes IN (SELECT TRY_CAST(TRIM(value) AS INT) FROM STRING_SPLIT(@MESES, ',') WHERE TRY_CAST(TRIM(value) AS INT) IS NOT NULL)
                	GROUP BY DimData.Data, IND_MES_ANO
                ),
                 ContaReceber (Data, MesAno, Quantidade, Valor, ValorManipulado) AS (
                	SELECT DimData.Data, IND_MES_ANO, COUNT(*) AS Quantidade, SUM(VAL_VALOR) AS Valor, SUM(VAL_VALOR_MANIPULADO) AS ValorManipulado
                		FROM CONTA_RECEBER
                			INNER JOIN DimData ON DimData.MesAno = CONTA_RECEBER.IND_MES_ANO AND Dia = 1
                			WHERE 1=1
                				AND DimData.Ano IN (SELECT TRY_CAST(TRIM(value) AS INT) FROM STRING_SPLIT(@ANOS, ',') WHERE TRY_CAST(TRIM(value) AS INT) IS NOT NULL)
                				AND DimData.Mes IN (SELECT TRY_CAST(TRIM(value) AS INT) FROM STRING_SPLIT(@MESES, ',') WHERE TRY_CAST(TRIM(value) AS INT) IS NOT NULL)
                	GROUP BY DimData.Data, IND_MES_ANO
                )
                
                	SELECT 
                			ContaPagar.Data              AS Date, 
                			ContaPagar.MesAno            AS MonthYear, 
                			ContaPagar.Quantidade        AS AccountsPayableQuantity, 
                			ContaPagar.Valor             AS AccountsPayableValue, 
                			ContaReceber.Quantidade      AS AccountsReceivableQuantity, 
                			ContaReceber.ValorManipulado AS AccountsReceivableValue 
                	FROM ContaPagar
                		INNER JOIN ContaReceber ON ContaPagar.MesAno = ContaReceber.MesAno

                   ORDER BY ContaPagar.Data";
    }
}