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
        private readonly Serilog.ILogger _appLogger;

        public QuickCaptureKeyHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserRepository userRepository,
            Serilog.ILogger appLogger)
            : base(options, logger, encoder, clock)
        {
            _userRepository = userRepository;
            // Logger da base (ILoggerFactory/Microsoft.Extensions.Logging) vai pro pipeline de logging
            // padrão do host, não pro arquivo Serilog que o resto do app usa (não há UseSerilog() aqui) —
            // por isso este handler loga com o mesmo Serilog.ILogger singleton usado em todo o resto do
            // app, senão os logs deste handler ficam invisíveis no arquivo de log de produção.
            _appLogger = appLogger;
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
                _appLogger.Warning(
                    "[QuickCaptureKeyHandler] - Header presente mas ignorado (chave vazia ou rota fora da allowlist). Rota: {Method} {Path}",
                    Request.Method, Request.Path);

                return AuthenticateResult.NoResult();
            }

            var hash = QuickCaptureKeyHasher.Hash(key);
            var user = await _userRepository.GetByQuickCaptureKeyHash(hash);

            if (user is null)
            {
                _appLogger.Warning(
                    "[QuickCaptureKeyHandler] - Chave de Lançamento Rápido inválida recebida para {Method} {Path}. Hash: {Hash}",
                    Request.Method, Request.Path, hash);

                return AuthenticateResult.Fail("Chave de Lançamento Rápido inválida.");
            }

            _appLogger.Information(
                "[QuickCaptureKeyHandler] - Autenticado via chave de Lançamento Rápido. UserId: {UserId}, Email: {Email}, Rota: {Method} {Path}, Hash: {Hash}",
                user.Id, user.Email, Request.Method, Request.Path, hash);

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
