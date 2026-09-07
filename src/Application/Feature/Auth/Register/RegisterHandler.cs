using Application.Feature.Auth.Common;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Application.Feature.Auth.Register
{
    /// <summary>
    /// Cadastro de um novo usuário (e-mail/senha). Inicia o trial gratuito e já devolve
    /// um access token, para o frontend logar automaticamente após o cadastro.
    /// </summary>
    public class RegisterHandler : IRegisterHandler
    {
        private readonly Serilog.ILogger _logger;
        private readonly IUserRepository _userRepository;
        private readonly JwtOptions _jwtOptions;
        private readonly TrialOptions _trialOptions;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public RegisterHandler(
            Serilog.ILogger logger,
            IUserRepository userRepository,
            IOptions<JwtOptions> jwtOptions,
            IOptions<TrialOptions> trialOptions)
        {
            _logger = logger;
            _userRepository = userRepository;
            _jwtOptions = jwtOptions.Value;
            _trialOptions = trialOptions.Value;
        }

        public async Task<RegisterOutput> Handle(RegisterInput input)
        {
            var validations = await RegisterValidator.ValidateInput(input, _userRepository);

            if (validations.Count > 0)
            {
                _logger.Warning("[RegisterHandler.Handle()] - Erro de validação no cadastro. Email: {Email}, Validação: {@Validations}", input.Email, validations);

                return new RegisterOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validations)
                };
            }

            var now = DateTime.UtcNow;

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = input.Email.Trim().ToLowerInvariant(),
                Name = input.Name.Trim(),
                CreatedAt = now,
                TrialStartsAt = now,
                TrialEndsAt = now.AddDays(_trialOptions.DurationDays),
                SubscriptionStatus = Domain.Entities.SubscriptionStatus.Trialing
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, input.Password);

            await _userRepository.Save(user);

            _logger.Information("[RegisterHandler.Handle()] - Novo usuário cadastrado. UserId: {UserId}", user.Id);

            var (accessToken, expiresInSeconds) = JwtAccessTokenFactory.Create(user.Id.ToString(), _jwtOptions);

            return new RegisterOutput
            {
                Output = OutputBaseDetails.Success("Cadastro realizado com sucesso.", new { user.Id, user.Email, user.Name }, 1),
                AccessToken = accessToken,
                ExpiresInSeconds = expiresInSeconds
            };
        }
    }
}
