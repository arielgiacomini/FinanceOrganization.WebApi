namespace Domain.Utils
{
    public class StringDecimalUtils
    {
        public static bool VerificaSeEhNumero(string valor)
        {
            if (decimal.TryParse(valor, out var _))
            {
                return true;
            }

            return false;
        }

        public static decimal TranslateStringEmDecimal(string valor, bool ehPercentual = false)
        {
            decimal result = default;
            bool flag = VerificaSeEhNumero(valor);
            if (valor.Contains("R$"))
            {
                result = Convert.ToDecimal(valor.Replace("R$ ", ""));
            }
            else if (valor.Contains(',') && !valor.Contains('%'))
            {
                result = Convert.ToDecimal(valor);
            }
            else if (valor.Contains('%'))
            {
                result = Convert.ToDecimal(valor.Replace("%", "")) / 100m;
            }
            else if (flag && !ehPercentual)
            {
                result = ((!decimal.TryParse(valor, out var _)) ? Convert.ToDecimal(valor) : Convert.ToDecimal(valor.Replace(".", ",")));
            }
            else if (flag && ehPercentual)
            {
                result = Convert.ToDecimal(valor);
                result /= 100m;
            }

            return result;
        }

        public static string TranslateValorEmStringDinheiro(string valor)
        {
            if (valor == "")
            {
                return 0.ToString("C");
            }

            if (valor == "R$ 0,00")
            {
                return valor;
            }

            if ((!valor.Contains("R$") || !valor.Contains(',')) && VerificaSeEhNumero(valor))
            {
                return Convert.ToDecimal(valor).ToString("C");
            }

            return valor;
        }
    }
}