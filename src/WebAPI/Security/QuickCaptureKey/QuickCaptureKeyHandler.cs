using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Application.Feature.Auth.Common;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebAPI.Security.QuickCaptureKey
{
    /// <summary>
    /// Autentica pela chave opaca de Lançamento Rápido (header X-Quick-Capture-Key), como
    /// alternativa ao Bearer JWT normal. Só autentica quando: (1) a rota+verbo da requisição está
    /// na allowlist fechada de QuickCaptureKeyDefaults, e (2) o hash da chave bate com algum
    /// usuário no banco. Fora disso a requisição segue não-autenticada — mesmo que a chave seja
    /// válida, enviá-la para edit/delete/pay/etc. não dá acesso (ver Startup.cs: o scheme só é
    /// selecionado para as rotas da allowlist; aqui a checagem é repetida como segunda camada).
    /// </summary>
    public class QuickCaptureKeyHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserRepository _userRepository;

        public QuickCaptureKeyHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserRepository userRepository)
            : base(options, logger, encoder, clock)
        {
            _userRepository = userRepository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(QuickCaptureKeyDefaults.HeaderName, out var headerValues))
            {
                return AuthenticateResult.NoResult();
            }

            var key = headerValues.ToString();

            if (string.IsNullOrWhiteSpace(key) || !QuickCaptureKeyDefaults.IsAllowedEndpoint(Request))
            {
                return AuthenticateResult.NoResult();
            }

            var hash = QuickCaptureKeyHasher.Hash(key);
            var user = await _userRepository.GetByQuickCaptureKeyHash(hash);

            if (user is null)
            {
                Logger.LogWarning("[QuickCaptureKeyHandler] - Chave de Lançamento Rápido inválida recebida para {Method} {Path}", Request.Method, Request.Path);

                return AuthenticateResult.Fail("Chave de Lançamento Rápido inválida.");
            }

            Logger.LogInformation(
                "[QuickCaptureKeyHandler] - Autenticado via chave de Lançamento Rápido. UserId: {UserId}, Email: {Email}, Rota: {Method} {Path}",
                user.Id, user.Email, Request.Method, Request.Path);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, QuickCaptureKeyDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, QuickCaptureKeyDefaults.AuthenticationScheme);

            return AuthenticateResult.Success(ticket);
        }
    }
}
