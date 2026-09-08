using Application.Feature.Auth.Common;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Application.Feature.Auth.PasswordGrant
{
    /// <summary>
    /// Grant OAuth2 "password" (RFC 6749 §4.3): troca email+senha por um access token que
    /// representa a pessoa (sub = UserId), diferente do client_credentials que representa o app.
    /// </summary>
    public class PasswordGrantHandler : IPasswordGrantHandler
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public PasswordGrantHandler(IOptions<JwtOptions> jwtOptions, IUserRepository userRepository)
        {
            _jwtOptions = jwtOptions.Value;
            _userRepository = userRepository;
        }

        public async Task<PasswordGrantOutput> Handle(PasswordGrantInput input)
        {
            var user = await _userRepository.GetByEmail(input.Email);

            if (user is null || string.IsNullOrEmpty(user.PasswordHash))
            {
                return new PasswordGrantOutput { Success = false };
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, input.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return new PasswordGrantOutput { Success = false };
            }

            var (accessToken, expiresInSeconds) = JwtAccessTokenFactory.Create(user.Id.ToString(), _jwtOptions);

            return new PasswordGrantOutput
            {
                Success = true,
                AccessToken = accessToken,
                ExpiresInSeconds = expiresInSeconds
            };
        }
    }
}
