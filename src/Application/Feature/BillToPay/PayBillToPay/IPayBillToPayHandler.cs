namespace Application.Feature.BillToPay.PayBillToPay
{
    public interface IPayBillToPayHandler
    {
        Task<PayBillToPayOutput> Handle(PayBillToPayInput input, CancellationToken cancellationToken);
    }
}