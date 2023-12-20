using Domain.Interfaces;
using FluentValidation;

namespace Application.Feature.BillToPay.UpdateBillToPay
{
    public class UpdateBillToPayValidator : AbstractValidator<UpdateBillToPayInput>
    {
        private readonly IWalletToPayRepository _walletToPayRepository;

        public UpdateBillToPayValidator(IWalletToPayRepository walletToPayRepository)
        {
            _walletToPayRepository = walletToPayRepository;

            RuleFor(input => input.Id)
                .NotEmpty()
                .NotNull()
                .WithMessage("O Guid Id não pode ser vazio ou nulo.");
            RuleFor(input => _walletToPayRepository!.GetBillToPayById(input.Id))
                .NotEmpty()
                .NotNull()
                .WithMessage("O id informado não existe na base de dados.");
        }
    }
}