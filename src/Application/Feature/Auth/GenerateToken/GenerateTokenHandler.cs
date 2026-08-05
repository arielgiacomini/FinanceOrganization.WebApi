using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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

            var expiresInSeconds = _jwtOptions.ExpirationMinutes * 60;
            var expiresAtUtc = DateTime.UtcNow.AddSeconds(expiresInSeconds);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, input.ClientId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expiresAtUtc,
                signingCredentials: signingCredentials);

            return new GenerateTokenOutput
            {
                Success = true,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
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