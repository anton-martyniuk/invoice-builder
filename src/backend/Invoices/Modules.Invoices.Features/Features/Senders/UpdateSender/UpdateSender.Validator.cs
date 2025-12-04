using FluentValidation;

namespace Modules.Invoices.Features.Features.Senders.UpdateSender;

public class UpdateSenderRequestValidator : AbstractValidator<UpdateSenderRequest>
{
    public UpdateSenderRequestValidator()
    {
        RuleFor(x => x.SenderCompanyName).NotEmpty();
        RuleFor(x => x.SenderFullName).NotEmpty();
        RuleFor(x => x.SenderAddress).NotEmpty();
        RuleFor(x => x.SenderTaxVatId).NotEmpty();
        RuleFor(x => x.BankDetails).NotEmpty();
    }
}
