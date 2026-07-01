namespace Domain.Entities.Dashboard
{
    public class MonthlyCashFlowDashboard
    {
        public string? Type { get; set; }
        public DateTime? Date { get; set; }
        public string? TaxCountry { get; set; }
        public string? MonthYear { get; set; }
        public int? Quantity { get; set; }
        public decimal? Value { get; set; }
        public decimal? ManipulatedValue { get; set; }
        public bool? HasPay { get; set; }
        public bool? HasReceivable { get; set; }
    }

    public class QuerySqlMonthlyCashFlowDashboard
    {
        private readonly string _years;
        private readonly string _months;
        private readonly string _foodVoucher;
        private readonly string _loanNextMonths;

        public QuerySqlMonthlyCashFlowDashboard(string? years, string? months, string? foodVoucher, string? loanNextMonths)
        {
            _years = string.IsNullOrEmpty(years) ? string.Concat(DateTime.Now.Year, ",", DateTime.Now.AddYears(1).Year) : years;
            _months = string.IsNullOrEmpty(months) ? "1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12" : months;
            _foodVoucher = foodVoucher ?? "'Vale Alimentação/Refeição'";
            _loanNextMonths = loanNextMonths ?? "'PLR - Ciclo 2 - 2025 de méritocracia (encerrando 2025)'";
        }

        /// <summary>
        /// Script SQL para buscar os dados do Dashboard Contas a Pagar/Receber por mês/ano, quantidade e valor.
        /// </summary>
        public FormattableString Sql => $@"
                DECLARE @ANOS_DEFAULT VARCHAR(10) = (SELECT CAST(YEAR(GETDATE()) AS VARCHAR(4)) AS MesesDefault);
DECLARE @ANOS VARCHAR(MAX) = {_years}
DECLARE @MESES VARCHAR(MAX) = {_months}
DECLARE @VALE VARCHAR(MAX) = {_foodVoucher}
DECLARE @EMPRESTIMO_PROX_MESES VARCHAR(MAX) = {_loanNextMonths}

;WITH ContaPagar (Data, PaisFiscal, MesAno, Quantidade, Valor, ValorManipulado, Pago, Recebido) 
AS (
	SELECT DimData.Data, DSC_PAIS_FISCAL AS PaisFiscal, IND_MES_ANO, COUNT(*) AS Quantidade, SUM(VAL_VALOR) AS Valor, CASE WHEN IND_PAGO = 0 THEN SUM(VAL_VALOR) ELSE 0 END AS ValorManipulado, IND_PAGO, NULL
		FROM CONTA_PAGAR
			INNER JOIN DimData ON DimData.MesAno = CONTA_PAGAR.IND_MES_ANO AND Dia = 1
			WHERE 1=1
				AND DimData.Ano IN (SELECT TRY_CAST(TRIM(value) AS INT) FROM STRING_SPLIT(@ANOS, ',') WHERE TRY_CAST(TRIM(value) AS INT) IS NOT NULL)
				AND DimData.Mes IN (SELECT TRY_CAST(TRIM(value) AS INT) FROM STRING_SPLIT(@MESES, ',') WHERE TRY_CAST(TRIM(value) AS INT) IS NOT NULL)
	GROUP BY DimData.Data, DSC_PAIS_FISCAL, IND_MES_ANO, IND_PAGO
),
 ContaReceber (Data, PaisFiscal, MesAno, Quantidade, Valor, ValorManipulado, Pago, Recebido) 
 AS (
	SELECT DimData.Data, DSC_PAIS_FISCAL AS PaisFiscal, IND_MES_ANO, COUNT(*) AS Quantidade, SUM(VAL_VALOR) AS Valor, SUM(VAL_VALOR_MANIPULADO) AS ValorManipulado, NULL, IND_RECEBIDO
		FROM CONTA_RECEBER
			INNER JOIN DimData ON DimData.MesAno = CONTA_RECEBER.IND_MES_ANO AND Dia = 1
			WHERE 1=1
				AND DimData.Ano IN (SELECT TRY_CAST(TRIM(value) AS INT) FROM STRING_SPLIT(@ANOS, ',') WHERE TRY_CAST(TRIM(value) AS INT) IS NOT NULL)
				AND DimData.Mes IN (SELECT TRY_CAST(TRIM(value) AS INT) FROM STRING_SPLIT(@MESES, ',') WHERE TRY_CAST(TRIM(value) AS INT) IS NOT NULL)
	GROUP BY DimData.Data, DSC_PAIS_FISCAL, IND_MES_ANO, IND_RECEBIDO
), ValeRefeicao (Data, PaisFiscal, MesAno, Quantidade, Valor, ValorManipulado, Pago, Recebido)
AS (
	SELECT DimData.Data, DSC_PAIS_FISCAL AS PaisFiscal, IND_MES_ANO, COUNT(*) AS Quantidade, SUM(VAL_VALOR) AS Valor, SUM(VAL_VALOR_MANIPULADO) AS ValorManipulado, NULL, IND_RECEBIDO
		FROM CONTA_RECEBER
			INNER JOIN DimData ON DimData.MesAno = CONTA_RECEBER.IND_MES_ANO AND Dia = 1
			WHERE 1=1
				AND DimData.Ano IN (SELECT TRY_CAST(TRIM(value) AS INT) FROM STRING_SPLIT(@ANOS, ',') WHERE TRY_CAST(TRIM(value) AS INT) IS NOT NULL)
				AND DimData.Mes IN (SELECT TRY_CAST(TRIM(value) AS INT) FROM STRING_SPLIT(@MESES, ',') WHERE TRY_CAST(TRIM(value) AS INT) IS NOT NULL)
				AND DSC_CATEGORIA IN (@VALE)
	GROUP BY DimData.Data, DSC_PAIS_FISCAL, IND_MES_ANO, IND_RECEBIDO
), EmprestimoProximosMeses (Data, PaisFiscal, MesAno, Quantidade, Valor, ValorManipulado, Pago, Recebido)
AS (
	SELECT DimData.Data, DSC_PAIS_FISCAL AS PaisFiscal, IND_MES_ANO, COUNT(*) AS Quantidade, SUM(VAL_VALOR) AS Valor, SUM(VAL_VALOR_MANIPULADO) AS ValorManipulado, NULL, IND_RECEBIDO
		FROM CONTA_RECEBER
			INNER JOIN DimData ON DimData.MesAno = CONTA_RECEBER.IND_MES_ANO AND Dia = 1
			WHERE 1=1
				--AND DimData.Ano IN (SELECT TRY_CAST(TRIM(value) AS INT) FROM STRING_SPLIT(@ANOS, ',') WHERE TRY_CAST(TRIM(value) AS INT) IS NOT NULL)
				--AND DimData.Mes IN (SELECT TRY_CAST(TRIM(value) AS INT) FROM STRING_SPLIT(@MESES, ',') WHERE TRY_CAST(TRIM(value) AS INT) IS NOT NULL)
				AND DSC_DESCRICAO IN (@EMPRESTIMO_PROX_MESES)
	GROUP BY DimData.Data, DSC_PAIS_FISCAL, IND_MES_ANO, IND_RECEBIDO
)
	SELECT 
		    '1 - Contas a Pagar Total' AS Type,
			ContaPagar.Data            AS Date,
			ContaPagar.PaisFiscal      AS TaxCountry,
			ContaPagar.MesAno          AS MonthYear,
			ContaPagar.Quantidade      AS Quantity, 
			ContaPagar.Valor           AS Value,
			ContaPagar.ValorManipulado AS ManipulatedValue,
			ContaPagar.Pago			   AS HasPay,
			ContaPagar.Recebido        AS HasReceivable
	FROM ContaPagar

	UNION ALL

	SELECT 
		    '2 - Contas a Receber Total' AS Tipo, 
			ContaReceber.Data            AS Date,
			ContaReceber.PaisFiscal      AS TaxCountry,
			ContaReceber.MesAno          AS MonthYear,
			ContaReceber.Quantidade      AS Quantity, 
			ContaReceber.Valor           AS Value, 
			ContaReceber.ValorManipulado AS ManipulatedValue,
			ContaReceber.Pago			 AS HasPay,
			ContaReceber.Recebido        AS HasReceivable
	FROM ContaReceber

	UNION ALL

	SELECT 
		    '3 - Vale Refeição/Alimentação' AS Tipo, 
			ValeRefeicao.Data               AS Date,
			ValeRefeicao.PaisFiscal         AS TaxCountry,
			ValeRefeicao.MesAno             AS MonthYear,
			ValeRefeicao.Quantidade         AS Quantity, 
			ValeRefeicao.Valor              AS Value, 
			ValeRefeicao.ValorManipulado    AS ManipulatedValue,
			ValeRefeicao.Pago			    AS HasPay,
			ValeRefeicao.Recebido           AS HasReceivable
	FROM ValeRefeicao

	UNION ALL

	SELECT 
		    '4 - Empréstimo Próximos Meses'         AS Tipo, 
			EmprestimoProximosMeses.Data            AS Date,
			EmprestimoProximosMeses.PaisFiscal      AS TaxCountry,
			EmprestimoProximosMeses.MesAno          AS MonthYear,
			EmprestimoProximosMeses.Quantidade      AS Quantity, 
			EmprestimoProximosMeses.Valor           AS Value, 
			EmprestimoProximosMeses.ValorManipulado AS ManipulatedValue,
			EmprestimoProximosMeses.Pago			AS HasPay,
			EmprestimoProximosMeses.Recebido        AS HasReceivable
	FROM EmprestimoProximosMeses

	ORDER BY Date";
    }
}