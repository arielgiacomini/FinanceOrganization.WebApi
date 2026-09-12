using Microsoft.AspNetCore.Http;

namespace WebAPI.Security.QuickCaptureKey
{
    /// <summary>
    /// Config central do bypass de autenticação da tela de Lançamento Rápido: nome do header, nome
    /// do scheme, e a allowlist fechada de rota+verbo onde a chave tem efeito. Único lugar que
    /// precisa mudar para adicionar/remover uma rota do escopo do bypass — nunca liberar aqui rotas
    /// de edição/exclusão/pagamento, só o que a tela de Lançamento Rápido precisa (criar + os dois
    /// dropdowns de conta/categoria).
    /// </summary>
    public static class QuickCaptureKeyDefaults
    {
        public const string AuthenticationScheme = "QuickCaptureKey";
        public const string HeaderName = "X-Quick-Capture-Key";

        public static readonly IReadOnlyList<(PathString Path, string Method)> AllowedEndpoints = new (PathString, string)[]
        {
            (new PathString("/v1/bills-to-pay/register"), HttpMethods.Post),
            (new PathString("/v1/account/search-all"), HttpMethods.Get),
            (new PathString("/v1/category/search"), HttpMethods.Get),
        };

        public static bool IsAllowedEndpoint(HttpRequest request)
        {
            foreach (var (path, method) in AllowedEndpoints)
            {
                if (request.Path.Equals(path, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(request.Method, method, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
