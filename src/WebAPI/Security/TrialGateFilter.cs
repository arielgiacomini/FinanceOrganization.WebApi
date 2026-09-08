using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Security
{
    /// <summary>
    /// Bloqueia com 402 (Payment Required) o acesso a qualquer endpoint autenticado quando o
    /// trial do usuário expirou e não há assinatura ativa. Ponto central — igual ao
    /// AuthorizeFilter global em Startup.cs — para não depender de checar isso tela por tela.
    /// Ainda não integra nenhum provedor de pagamento: só lê User.SubscriptionStatus/TrialEndsAt,
    /// que futuramente um webhook (Lemon Squeezy/Paddle/Stripe) vai manter atualizado.
    /// </summary>
    public class TrialGateFilter : IAsyncActionFilter
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;

        public TrialGateFilter(ICurrentUserService currentUserService, IUserRepository userRepository)
        {
            _currentUserService = currentUserService;
            _userRepository = userRepository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Projeto usa EnableEndpointRouting = false: [AllowAnonymous] chega como
            // IAllowAnonymousFilter em context.Filters, não em EndpointMetadata.
            var isAnonymousEndpoint = context.Filters.Any(filter => filter is IAllowAnonymousFilter);

            if (isAnonymousEndpoint || _currentUserService.UserId is not Guid userId)
            {
                await next();
                return;
            }

            var user = await _userRepository.GetById(userId);

            if (user is null || !HasAccess(user))
            {
                context.Result = new ObjectResult(new
                {
                    error = "subscription_required",
                    message = "Seu período de teste terminou. Assine para continuar usando o app."
                })
                {
                    StatusCode = StatusCodes.Status402PaymentRequired
                };

                return;
            }

            await next();
        }

        private static bool HasAccess(User user)
        {
            if (user.SubscriptionStatus == SubscriptionStatus.Active)
            {
                return true;
            }

            return user.SubscriptionStatus == SubscriptionStatus.Trialing && user.TrialEndsAt > DateTime.UtcNow;
        }
    }
}
