using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Feature.Auth.Common
{
    /// <summary>
    /// Construção do JWT de acesso, compartilhada por todos os grants (client_credentials,
    /// password, google) para não duplicar a lógica de assinatura/claims em cada handler.
    /// </summary>
    public static class JwtAccessTokenFactory
    {
        public static (string AccessToken, int ExpiresInSeconds) Create(string subject, JwtOptions jwtOptions)
        {
            var expiresInSeconds = jwtOptions.ExpirationMinutes * 60;
            var expiresAtUtc = DateTime.UtcNow.AddSeconds(expiresInSeconds);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                expires: expiresAtUtc,
                signingCredentials: signingCredentials);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresInSeconds);
        }
    }
}
