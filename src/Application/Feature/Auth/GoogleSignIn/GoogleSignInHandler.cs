using Application.Feature.Auth.Common;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.Extensions.Options;

namespace Application.Feature.Auth.GoogleSignIn
{
    /// <summary>
    /// Login/cadastro via Google Identity Services: valida o id_token vindo do frontend,
    /// acha o usuário pelo GoogleSub (ou pelo e-mail, para linkar uma conta já existente),
    /// cria se for a primeira vez, e emite o mesmo tipo de access token do login por senha.
    /// </summary>
    public class GoogleSignInHandler : IGoogleSignInHandler
    {
        private readonly Serilog.ILogger _logger;
        private readonly IUserRepository _userRepository;
        private readonly IGoogleIdTokenValidator _googleIdTokenValidator;
        private readonly JwtOptions _jwtOptions;
        private readonly TrialOptions _trialOptions;

        public GoogleSignInHandler(
            Serilog.ILogger logger,
            IUserRepository userRepository,
            IGoogleIdTokenValidator googleIdTokenValidator,
            IOptions<JwtOptions> jwtOptions,
            IOptions<TrialOptions> trialOptions)
        {
            _logger = logger;
            _userRepository = userRepository;
            _googleIdTokenValidator = googleIdTokenValidator;
            _jwtOptions = jwtOptions.Value;
            _trialOptions = trialOptions.Value;
        }

        public async Task<GoogleSignInOutput> Handle(GoogleSignInInput input)
        {
            var googleUser = await _googleIdTokenValidator.ValidateAsync(input.IdToken);

            if (googleUser is null)
            {
                _logger.Warning("[GoogleSignInHandler.Handle()] - id_token do Google inválido ou expirado.");

                return new GoogleSignInOutput { Success = false };
            }

            var user = await _userRepository.GetByGoogleSub(googleUser.Subject)
                ?? await _userRepository.GetByEmail(googleUser.Email);

            if (user is null)
            {
                var now = DateTime.UtcNow;

                user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = googleUser.Email.Trim().ToLowerInvariant(),
                    Name = googleUser.Name,
                    GoogleSub = googleUser.Subject,
                    CreatedAt = now,
                    TrialStartsAt = now,
                    TrialEndsAt = now.AddDays(_trialOptions.DurationDays),
                    SubscriptionStatus = Domain.Entities.SubscriptionStatus.Trialing
                };

                await _userRepository.Save(user);

                _logger.Information("[GoogleSignInHandler.Handle()] - Novo usuário cadastrado via Google. UserId: {UserId}", user.Id);
            }
            else if (user.GoogleSub is null)
            {
                user.GoogleSub = googleUser.Subject;
                await _userRepository.Edit(user);
            }

            var (accessToken, expiresInSeconds) = JwtAccessTokenFactory.Create(user.Id.ToString(), _jwtOptions);

            return new GoogleSignInOutput
            {
                Success = true,
                AccessToken = accessToken,
                ExpiresInSeconds = expiresInSeconds
            };
        }
    }
}
