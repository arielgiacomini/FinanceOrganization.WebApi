using System.IdentityModel.Tokens.Jwt;
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
            var subjectClaim = httpContextAccessor.HttpContext?.User?
                .FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            UserId = Guid.TryParse(subjectClaim, out var userId) ? userId : null;
        }
    }
}
