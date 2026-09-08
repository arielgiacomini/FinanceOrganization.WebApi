using System.Security.Cryptography;
using System.Text;
using Application.Feature.Auth.Common;
using Domain.Options;
using Microsoft.Extensions.Options;

namespace Application.Feature.Auth.GenerateToken
{
    public class GenerateTokenHandler : IGenerateTokenHandler
    {
        private readonly JwtOptions _jwtOptions;
        private readonly AuthClientOptions _authClientOptions;

        public GenerateTokenHandler(IOptions<JwtOptions> jwtOptions, IOptions<AuthClientOptions> authClientOptions)
        {
            _jwtOptions = jwtOptions.Value;
            _authClientOptions = authClientOptions.Value;
        }

        public GenerateTokenOutput Handle(GenerateTokenInput input)
        {
            if (!AreCredentialsValid(input))
            {
                return new GenerateTokenOutput { Success = false };
            }

            // client_credentials representa "o app", não uma pessoa: o sub é o dono configurado
            // (usado só durante a transição do frontend legado para login por usuário). Sem
            // OwnerUserId configurado, o token não enxerga dado nenhum (ver ICurrentUserService).
            var subject = _authClientOptions.OwnerUserId?.ToString() ?? input.ClientId;

            var (accessToken, expiresInSeconds) = JwtAccessTokenFactory.Create(subject, _jwtOptions);

            return new GenerateTokenOutput
            {
                Success = true,
                AccessToken = accessToken,
                ExpiresInSeconds = expiresInSeconds
            };
        }

        private bool AreCredentialsValid(GenerateTokenInput input)
        {
            return FixedTimeEquals(input.ClientId, _authClientOptions.ClientId)
                && FixedTimeEquals(input.ClientSecret, _authClientOptions.ClientSecret);
        }

        private static bool FixedTimeEquals(string left, string right)
        {
            var leftBytes = Encoding.UTF8.GetBytes(left ?? string.Empty);
            var rightBytes = Encoding.UTF8.GetBytes(right ?? string.Empty);

            if (leftBytes.Length != rightBytes.Length)
            {
                return false;
            }

            return CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
        }
    }
}