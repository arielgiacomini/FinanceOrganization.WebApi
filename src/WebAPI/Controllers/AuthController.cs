using System.Text;
using Application.Feature.Auth.GenerateToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("v1/auth")]
    [Produces("application/json")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IGenerateTokenHandler _generateTokenHandler;

        public AuthController(Serilog.ILogger logger, IGenerateTokenHandler generateTokenHandler)
        {
            _logger = logger;
            _generateTokenHandler = generateTokenHandler;
        }

        /// <summary>
        /// Gera um access token JWT via OAuth 2.0 Client Credentials Grant (RFC 6749 §4.4).
        /// Aceita as credenciais no corpo (client_id/client_secret) ou via Authorization: Basic.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("token")]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult GenerateToken([FromForm] OAuthTokenRequest request)
        {
            if (!string.Equals(request.GrantType, "client_credentials", StringComparison.Ordinal))
            {
                return BadRequest(new { error = "unsupported_grant_type" });
            }

            var (clientId, clientSecret) = ExtractClientCredentials(request);

            var output = _generateTokenHandler.Handle(new GenerateTokenInput
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            });

            if (!output.Success)
            {
                _logger.Warning("[AuthController.GenerateToken()] - Tentativa de autenticação com credenciais inválidas para o ClientId: {ClientId}", clientId);

                return BadRequest(new { error = "invalid_client" });
            }

            return Ok(new
            {
                access_token = output.AccessToken,
                token_type = "Bearer",
                expires_in = output.ExpiresInSeconds
            });
        }

        private (string ClientId, string ClientSecret) ExtractClientCredentials(OAuthTokenRequest request)
        {
            var authorizationHeader = Request.Headers.Authorization.ToString();

            if (authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var encodedCredentials = authorizationHeader["Basic ".Length..];
                    var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                    var separatorIndex = decodedCredentials.IndexOf(':');

                    if (separatorIndex >= 0)
                    {
                        return (decodedCredentials[..separatorIndex], decodedCredentials[(separatorIndex + 1)..]);
                    }
                }
                catch (FormatException)
                {
                    // Cabeçalho Basic malformado: cai para as credenciais do corpo (provavelmente vazias -> invalid_client).
                }
            }

            return (request.ClientId ?? string.Empty, request.ClientSecret ?? string.Empty);
        }
    }
}
