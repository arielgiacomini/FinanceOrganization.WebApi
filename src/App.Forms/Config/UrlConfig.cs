using System.Configuration;

namespace App.Forms.Config
{
    public static class UrlConfig
    {
        private const string URL_CASE_DEFAULT = "http://api.financeiro.arielgiacomini.com.br";

        public static string? GetFinanceOrganizationApiUrl(string? environment)
        {
            if (string.IsNullOrWhiteSpace(environment))
            {
                environment = "Homologação";
            }

            string? configUrl = environment switch
            {
                "Produção" => ConfigurationManager.AppSettings["finance-organization-producao-api-url"],
                "Homologação" => ConfigurationManager.AppSettings["finance-organization-homologacao-api-url"],
                "Local" => ConfigurationManager.AppSettings["finance-organization-local-api-url"],
                _ => URL_CASE_DEFAULT,
            };

            return configUrl;
        }
    }
}