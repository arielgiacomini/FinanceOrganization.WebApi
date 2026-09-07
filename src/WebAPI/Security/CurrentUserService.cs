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
    public class CurrentUserService : ICurrentUserService
    {
        public Guid? UserId { get; }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext?.User;

            // Por padrão o JwtBearerOptions.MapInboundClaims (desligado em Startup.cs) já mantém o
            // claim como "sub". Ainda assim checa também ClaimTypes.NameIdentifier (o URI legado pro
            // qual "sub" é remapeado quando esse flag está ligado) — se isso mudar de novo no futuro,
            // a identidade do usuário não deve quebrar silenciosamente (filtro de isolamento -> 0 linhas).
            var subjectClaim = user?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            UserId = Guid.TryParse(subjectClaim, out var userId) ? userId : null;
        }
    }
}
