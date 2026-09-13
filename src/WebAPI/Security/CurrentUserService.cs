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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid? _explicitUserId;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Lida sob demanda, a cada leitura — nunca em cache no construtor. Um scheme de autenticação
        /// que faz I/O (ex.: QuickCaptureKeyHandler, que consulta o banco pelo hash da chave) só termina
        /// de popular HttpContext.User DEPOIS que este serviço pode já ter sido construído (DI resolve
        /// os filtros/serviços da request antes da autenticação assíncrona terminar). Calcular o UserId
        /// uma única vez no construtor "fotografava" o usuário como anônimo nesse instante, e esse valor
        /// (errado) ficava congelado pro resto da requisição — mesmo a autenticação tendo sucesso logo
        /// em seguida. Lendo sempre ao vivo, o valor reflete o HttpContext.User no momento real do uso
        /// (dentro do filtro global de isolamento do FinanceOrganizationContext), não no momento em que
        /// este serviço foi instanciado.
        /// </summary>
        public Guid? UserId
        {
            get
            {
                if (_explicitUserId.HasValue)
                {
                    return _explicitUserId;
                }

                var user = _httpContextAccessor.HttpContext?.User;

                // Por padrão o JwtBearerOptions.MapInboundClaims (desligado em Startup.cs) já mantém o
                // claim como "sub". Ainda assim checa também ClaimTypes.NameIdentifier (o URI legado pro
                // qual "sub" é remapeado quando esse flag está ligado) — se isso mudar de novo no futuro,
                // a identidade do usuário não deve quebrar silenciosamente (filtro de isolamento -> 0 linhas).
                var subjectClaim = user?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                    ?? user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return Guid.TryParse(subjectClaim, out var userId) ? userId : null;
            }
        }

        /// <summary>
        /// Só para uso fora de uma requisição HTTP (ex.: GenericBackgroundServices) — não existe token
        /// nem HttpContext ali, então o "usuário atual" precisa ser atribuído explicitamente, um de cada
        /// vez, dentro de um escopo de DI próprio criado pra esse usuário. Não expor isso na interface
        /// ICurrentUserService: nenhum código de request HTTP deveria poder trocar o usuário no meio do caminho.
        /// </summary>
        public void SetUserId(Guid userId) => _explicitUserId = userId;
    }
}
