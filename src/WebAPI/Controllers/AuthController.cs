using System.Text;
using Application.Feature.Auth.GenerateToken;
using Application.Feature.Auth.GoogleSignIn;
using Application.Feature.Auth.PasswordGrant;
using Application.Feature.Auth.Register;
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
        private readonly IPasswordGrantHandler _passwordGrantHandler;
        private readonly IRegisterHandler _registerHandler;
        private readonly IGoogleSignInHandler _googleSignInHandler;

        public AuthController(
            Serilog.ILogger logger,
            IGenerateTokenHandler generateTokenHandler,
            IPasswordGrantHandler passwordGrantHandler,
            IRegisterHandler registerHandler,
            IGoogleSignInHandler googleSignInHandler)
        {
            _logger = logger;
            _generateTokenHandler = generateTokenHandler;
            _passwordGrantHandler = passwordGrantHandler;
            _registerHandler = registerHandler;
            _googleSignInHandler = googleSignInHandler;
        }

        /// <summary>
        /// Gera um access token JWT via OAuth 2.0 (RFC 6749). Suporta dois grants:
        /// - client_credentials (§4.4): representa "o app" — legado, em transição.
        /// - password (§4.3): representa uma pessoa (username=e-mail, password=senha).
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("token")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> GenerateToken([FromForm] OAuthTokenRequest request)
        {
            return request.GrantType switch
            {
                "client_credentials" => HandleClientCredentials(request),
                "password" => await HandlePassword(request),
                _ => BadRequest(new { error = "unsupported_grant_type" })
            };
        }

        /// <summary>
        /// Cadastro de um novo usuário por e-mail/senha. Já devolve um access token.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            _logger.Information("[AuthController.Register()] - Cadastro de novo usuário. Email: {Email}", request.Email);

            var output = await _registerHandler.Handle(new RegisterInput
            {
                Email = request.Email,
                Name = request.Name,
                Password = request.Password
            });

            if (output.Output.Status == Application.Feature.OutputBaseDetails.OutputStatus.HasValidationIssue)
            {
                return BadRequest(output);
            }

            return Ok(output);
        }

        /// <summary>
        /// Login/cadastro via Google Identity Services: recebe o id_token obtido pelo frontend
        /// e devolve um access token próprio (mesmo formato do login por senha).
        /// </summary>
        [HttpPost("google")]
        public async Task<IActionResult> Google([FromBody] GoogleSignInRequest request)
        {
            var output = await _googleSignInHandler.Handle(new GoogleSignInInput { IdToken = request.IdToken });

            if (!output.Success)
            {
                return BadRequest(new { error = "invalid_token" });
            }

            return Ok(new
            {
                access_token = output.AccessToken,
                token_type = "Bearer",
                expires_in = output.ExpiresInSeconds
            });
        }

        private IActionResult HandleClientCredentials(OAuthTokenRequest request)
        {
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

        private async Task<IActionResult> HandlePassword(OAuthTokenRequest request)
        {
            var output = await _passwordGrantHandler.Handle(new PasswordGrantInput
            {
                Email = request.Username ?? string.Empty,
                Password = request.Password ?? string.Empty
            });

            if (!output.Success)
            {
                _logger.Warning("[AuthController.GenerateToken()] - Tentativa de login com credenciais inválidas para o e-mail: {Email}", request.Username);

                return BadRequest(new { error = "invalid_grant" });
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
