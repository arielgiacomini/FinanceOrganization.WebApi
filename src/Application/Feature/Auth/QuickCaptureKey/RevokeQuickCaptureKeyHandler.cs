using Domain.Interfaces;

namespace Application.Feature.Auth.QuickCaptureKey
{
    /// <summary>
    /// Revoga a chave de Lançamento Rápido do usuário autenticado (ex.: suspeita de vazamento).
    /// Idempotente: chamar sem uma chave ativa não é erro.
    /// </summary>
    public class RevokeQuickCaptureKeyHandler : IRevokeQuickCaptureKeyHandler
    {
        private readonly Serilog.ILogger _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;

        public RevokeQuickCaptureKeyHandler(
            Serilog.ILogger logger,
            ICurrentUserService currentUserService,
            IUserRepository userRepository)
        {
            _logger = logger;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
        }

        public async Task Handle()
        {
            if (_currentUserService.UserId is not Guid userId)
            {
                return;
            }

            var user = await _userRepository.GetById(userId);

            if (user is null || user.QuickCaptureKeyHash is null)
            {
                return;
            }

            user.QuickCaptureKeyHash = null;
            user.QuickCaptureKeyCreatedAt = null;

            await _userRepository.Edit(user);

            _logger.Information("[RevokeQuickCaptureKeyHandler.Handle()] - Chave de Lançamento Rápido revogada. UserId: {UserId}", userId);
        }
    }
}
