using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Interfaces;

namespace WebAPI.Security
{
    /// <summary>
    /// Lê o UserId (claim "sub") do token JWT já validado da requisição HTTP atual.
    /// Único ponto de leitura de identidade do usuário — o filtro global de isolamento
    /// no FinanceOrganizationContext depende exclusivamente disto.
    /// </summary>
    public class CurrentUserService : ICurrentUserService, ICurrentUserSetter
    {
        public Guid? UserId { get; private set; }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, Serilog.ILogger logger)
        {
            var user = httpContextAccessor.HttpContext?.User;

            // Por padrão o JwtBearerOptions.MapInboundClaims (desligado em Startup.cs) já mantém o
            // claim como "sub". Ainda assim checa também ClaimTypes.NameIdentifier (o URI legado pro
            // qual "sub" é remapeado quando esse flag está ligado) — se isso mudar de novo no futuro,
            // a identidade do usuário não deve quebrar silenciosamente (filtro de isolamento -> 0 linhas).
            var subjectClaim = user?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            UserId = Guid.TryParse(subjectClaim, out var userId) ? userId : null;

            // Diagnóstico temporário (investigação da chave de Lançamento Rápido devolvendo dado vazio):
            // esse é o único ponto de verdade da identidade em toda a request, então logar aqui mostra
            // exatamente o que qualquer fluxo de autenticação (Bearer, chave, etc.) resolveu.
            logger.Information(
                "[CurrentUserService] - UserId resolvido: {UserId} (claim sub bruta: {SubjectClaim}, autenticado: {IsAuthenticated}, scheme: {AuthType}, path: {Path})",
                UserId, subjectClaim, user?.Identity?.IsAuthenticated, user?.Identity?.AuthenticationType,
                httpContextAccessor.HttpContext?.Request?.Path);
        }

        /// <summary>
        /// Só para uso fora de uma requisição HTTP (ex.: GenericBackgroundServices) — não existe token
        /// nem HttpContext ali, então o "usuário atual" precisa ser atribuído explicitamente, um de cada
        /// vez, dentro de um escopo de DI próprio criado pra esse usuário. Não expor isso na interface
        /// ICurrentUserService: nenhum código de request HTTP deveria poder trocar o usuário no meio do caminho.
        /// </summary>
        public void SetUserId(Guid userId) => UserId = userId;
    }
}
