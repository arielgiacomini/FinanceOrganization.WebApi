using Domain.Entities.Extern;
using Domain.Interfaces;
using Domain.Options;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using Serilog;

namespace Infrastructure.Auth
{
    public class GoogleIdTokenValidator : IGoogleIdTokenValidator
    {
        private readonly GoogleAuthOptions _googleAuthOptions;
        private readonly ILogger _logger;

        public GoogleIdTokenValidator(IOptions<GoogleAuthOptions> googleAuthOptions, ILogger logger)
        {
            _googleAuthOptions = googleAuthOptions.Value;
            _logger = logger;
        }

        public async Task<GoogleUserInfo?> ValidateAsync(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _googleAuthOptions.ClientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                return new GoogleUserInfo
                {
                    Subject = payload.Subject,
                    Email = payload.Email,
                    Name = payload.Name
                };
            }
            catch (InvalidJwtException ex)
            {
                _logger.Warning(ex, "[GoogleIdTokenValidator.ValidateAsync()] - id_token do Google rejeitado.");

                return null;
            }
        }
    }
}
