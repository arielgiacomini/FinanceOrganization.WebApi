using App.Forms.Config;
using App.Forms.Forms;

namespace App.Forms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Initial(GetInfoHeader()));
        }

        private static InfoHeader? GetInfoHeader()
        {
            var urlAPI = UrlConfig.GetFinanceOrganizationApiUrl("Produção");

            if (string.IsNullOrWhiteSpace(urlAPI))
            {
                return null;
            }

            bool productionEnviroment = false;

            if (urlAPI.StartsWith("http://api.financeiro.arielgiacomini.com.br"))
            {
                productionEnviroment = true;
            }

            var infoHeader = new InfoHeader
            {
                IsProductionEnvironment = productionEnviroment,
                Url = urlAPI,
                Version = Info.GetVersionString(),
                Environment = "Produção"
            };

            return infoHeader;
        }
    }
}