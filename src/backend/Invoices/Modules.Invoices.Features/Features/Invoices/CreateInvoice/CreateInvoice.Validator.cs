using FluentValidation;
using Modules.Invoices.Features.Features.Shared.Requests;

namespace Modules.Invoices.Features.Features.Invoices.CreateInvoice;

public class CreateInvoiceRequestValidator : AbstractValidator<CreateInvoiceRequest>
{
	public CreateInvoiceRequestValidator()
	{
		RuleFor(invoice => invoice.InvoiceNumber).NotEmpty();
		RuleFor(invoice => invoice.InvoiceDate).NotEmpty();
		RuleFor(invoice => invoice.DueDate).NotEmpty().GreaterThanOrEqualTo(invoice => invoice.InvoiceDate);
		RuleFor(invoice => invoice.Currency).NotEmpty();
		RuleFor(invoice => invoice.Notes).NotEmpty();
		RuleFor(invoice => invoice.CustomerId).NotEmpty();
		RuleFor(invoice => invoice.SenderId).NotEmpty();
		RuleFor(invoice => invoice.Subtotal).GreaterThan(0);
		RuleFor(invoice => invoice.TaxRate).GreaterThanOrEqualTo(0);
		RuleFor(invoice => invoice.TotalAmount).GreaterThan(0);
		RuleFor(invoice => invoice.LineItems).NotEmpty();

		RuleForEach(invoice => invoice.LineItems)
			.SetValidator(new InvoiceLineItemRequestValidator());
	}
}

public class InvoiceLineItemRequestValidator : AbstractValidator<InvoiceLineItemRequest>
{
	public InvoiceLineItemRequestValidator()
	{
		RuleFor(item => item.ItemName).NotEmpty();
		RuleFor(item => item.Quantity).GreaterThan(0);
		RuleFor(item => item.UnitPrice).GreaterThan(0);
		RuleFor(item => item.Total).GreaterThan(0);
	}
}
