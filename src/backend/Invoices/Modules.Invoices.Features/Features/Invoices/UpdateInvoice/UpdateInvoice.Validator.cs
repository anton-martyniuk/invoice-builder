using FluentValidation;

namespace Modules.Invoices.Features.Features.Invoices.UpdateInvoice;

public class UpdateInvoiceRequestValidator : AbstractValidator<UpdateInvoiceRequest>
{
	public UpdateInvoiceRequestValidator()
	{
		RuleFor(x => x.InvoiceNumber).NotEmpty();
		RuleFor(x => x.InvoiceDate).NotEmpty();
		RuleFor(x => x.DueDate)
			.NotEmpty()
			.GreaterThanOrEqualTo(x => x.InvoiceDate)
			.WithMessage("Due date must be greater than or equal to invoice date");
		RuleFor(x => x.Currency).NotEmpty();
		RuleFor(x => x.CustomerId).NotEmpty();
		RuleFor(x => x.SenderId).NotEmpty();
		RuleFor(x => x.Subtotal).GreaterThan(0);
		RuleFor(x => x.TaxRate).GreaterThanOrEqualTo(0);
		RuleFor(x => x.TotalAmount).GreaterThan(0);
	}
}
