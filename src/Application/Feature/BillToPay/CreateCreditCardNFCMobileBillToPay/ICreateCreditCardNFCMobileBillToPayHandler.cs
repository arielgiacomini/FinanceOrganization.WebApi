
namespace Application.Feature.BillToPay.CreateCreditCardNFCMobileBillToPay
{
    public interface ICreateCreditCardNFCMobileBillToPayHandler
    {
        Task<CreateCreditCardNFCMobileBillToPayOutput> Handle(CreateCreditCardNFCMobileBillToPayInput input, CancellationToken cancellationToken = default);
    }
}