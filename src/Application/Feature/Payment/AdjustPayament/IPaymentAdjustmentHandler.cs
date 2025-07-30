

namespace Application.Feature.Payment.AdjustPayament
{
    public interface IPaymentAdjustmentHandler
    {
        Task<bool> ConsideredPaid(string accountName, string registrationType);
        Task<PaymentAdjustmentOutput> Handle(PaymentAdjustmentInput input, CancellationToken cancellationToken);
    }
}