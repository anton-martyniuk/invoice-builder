using FluentValidation;

namespace Modules.Invoices.Features.Features.Senders.CreateSender;

public class CreateSenderRequestValidator : AbstractValidator<CreateSenderRequest>
{
    public CreateSenderRequestValidator()
    {
        RuleFor(x => x.SenderCompanyName).NotEmpty();
        RuleFor(x => x.SenderFullName).NotEmpty();
        RuleFor(x => x.SenderAddress).NotEmpty();
        RuleFor(x => x.SenderTaxVatId).NotEmpty();
        RuleFor(x => x.BankDetails).NotEmpty();
    }
}
