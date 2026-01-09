namespace Domain.Entities.Dashboard
{
    public class DailyExpenseByCategoryDateDashboard
    {
        /// <summary>
        /// Descrição da Categoria
        /// </summary>
        public string? Category { get; set; }
        /// <summary>
        /// Mês e Ano - Padrão: "Janeiro/2026"
        /// </summary>
        public string? MonthYear { get; set; }
        /// <summary>
        /// Indica em texto a semana do mês
        /// </summary>
        public string? WeekName { get; set; }
        /// <summary>
        /// Data
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// Indica se é a semana atual
        /// </summary>
        public bool? CurrentWeek { get; set; }
        /// <summary>
        /// Valor em R$ gasto
        /// </summary>
        public decimal? ValueSpent { get; set; }
    }

    public class QuerySqlDailyExpenseByCategoryDateDashboard
    {
        public static string? Category { get; set; }
        public static string? YearMonth { get; set; }

        public QuerySqlDailyExpenseByCategoryDateDashboard(string yearMonth, string category)
        {
            Category = category;
            YearMonth = yearMonth;
        }


        /// <summary>
        /// Script SQL para buscar os dados do dashboard
        /// </summary>
        public FormattableString Sql => $@"
			DECLARE @Category VARCHAR(100) = {Category ?? "NULL"};
			DECLARE @MonthYear VARCHAR(100) = {YearMonth ?? "NULL"};

				SELECT 
                    CAST(ISNULL(GastosRealizados.DSC_CATEGORIA, @Category) AS VARCHAR(100)) AS Category,
                    CAST(DimData.MesAno AS VARCHAR(100)) AS MonthYear,
                    CAST('Semana: ' + CAST(DENSE_RANK() OVER(PARTITION BY DimData.MesAno ORDER BY DimData.SemanaAno ASC) AS VARCHAR) 
                    		+ ' - entre: ' + LEFT(CONVERT(VARCHAR(10), DadosSemana.PrimeiraData, 103), 5) + ' e ' + LEFT(CONVERT(VARCHAR(10), DadosSemana.UltimaData, 103), 5) AS VARCHAR(MAX)) AS WeekName,
                    CAST(DimData.Data AS DATE) AS Date,
                    CAST(CASE WHEN CAST(GETDATE() AS DATE) BETWEEN DadosSemana.PrimeiraData AND DadosSemana.UltimaData THEN 1 ELSE 0 END AS BIT) AS CurrentWeek,
                    SUM(ISNULL(CAST(GastosRealizados.ValorDia AS NUMERIC(18, 2)), 0.0)) AS ValueSpent
	        FROM DimData
			LEFT JOIN (
							SELECT Ano, Mes, SemanaAno, MIN(Data) AS PrimeiraData, MAX(Data) AS UltimaData FROM DimData AS DimDataDentro
								WHERE 1=1
							GROUP BY Ano, Mes, SemanaAno
			) AS DadosSemana ON DadosSemana.Ano = DimData.Ano AND DadosSemana.Mes = DimData.Mes AND DadosSemana.SemanaAno = DimData.SemanaAno
			LEFT JOIN (
							SELECT Ano, Mes, Data, SemanaAno, MIN(Data) AS PrimeiraData, MAX(Data) AS UltimaData FROM DimData AS DimDataDentro
								WHERE 1=1
							GROUP BY Ano, Mes, Data, SemanaAno
			) AS DadosDia ON DadosDia.Data = DimData.Data
			LEFT JOIN (
		    SELECT 
		        DimDataDentro.Ano,
		        DimDataDentro.Mes,
		        DimDataDentro.Data,
				DSC_CATEGORIA,
		        SUM(ISNULL(Dentro.VAL_VALOR, 0)) AS ValorDia
		    FROM CONTA_PAGAR AS Dentro
		    INNER JOIN DimData AS DimDataDentro 
		        ON DimDataDentro.Data = Dentro.DAT_COMPRA
		    WHERE (@Category IS NULL OR Dentro.DSC_CATEGORIA = @Category)
		      AND Dentro.DSC_DESCRICAO NOT LIKE '%Projetado%'
		    GROUP BY 
		        DimDataDentro.Ano,
		        DimDataDentro.Mes,
		        DimDataDentro.Data,
				DSC_CATEGORIA
			) AS GastosRealizados 
			ON GastosRealizados.Ano = DimData.Ano
			AND GastosRealizados.Mes = DimData.Mes
			AND GastosRealizados.Data = DimData.Data
		WHERE 1=1
			AND (@MonthYear IS NULL OR DimData.MesAno = @MonthYear)
	    GROUP BY GastosRealizados.DSC_CATEGORIA, DimData.MesAno, DadosSemana.SemanaAno, DimData.SemanaAno, DimData.Data, DadosSemana.PrimeiraData, DadosSemana.UltimaData, DadosDia.PrimeiraData, DadosDia.UltimaData";
    }
}