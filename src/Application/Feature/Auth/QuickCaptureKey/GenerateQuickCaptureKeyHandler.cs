using Application.Feature.Auth.Common;
using Domain.Interfaces;

namespace Application.Feature.Auth.QuickCaptureKey
{
    /// <summary>
    /// Gera (ou substitui) a chave opaca de Lançamento Rápido do usuário autenticado atualmente.
    /// Gerar uma nova invalida a anterior automaticamente (só existe um hash por usuário).
    /// </summary>
    public class GenerateQuickCaptureKeyHandler : IGenerateQuickCaptureKeyHandler
    {
        private readonly Serilog.ILogger _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;

        public GenerateQuickCaptureKeyHandler(
            Serilog.ILogger logger,
            ICurrentUserService currentUserService,
            IUserRepository userRepository)
        {
            _logger = logger;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
        }

        public async Task<GenerateQuickCaptureKeyOutput> Handle()
        {
            if (_currentUserService.UserId is not Guid userId)
            {
                return new GenerateQuickCaptureKeyOutput { Success = false };
            }

            var user = await _userRepository.GetById(userId);

            if (user is null)
            {
                return new GenerateQuickCaptureKeyOutput { Success = false };
            }

            var plainKey = QuickCaptureKeyHasher.GenerateKey();

            user.QuickCaptureKeyHash = QuickCaptureKeyHasher.Hash(plainKey);
            user.QuickCaptureKeyCreatedAt = DateTime.UtcNow;

            await _userRepository.Edit(user);

            _logger.Information("[GenerateQuickCaptureKeyHandler.Handle()] - Nova chave de Lançamento Rápido gerada. UserId: {UserId}", userId);

            return new GenerateQuickCaptureKeyOutput
            {
                Success = true,
                QuickCaptureKey = plainKey
            };
        }
    }
}
