namespace Modules.Invoices.Features.Features.Shared.Responses;

public sealed record InvoiceResponse(
	Guid Id,
	string InvoiceNumber,
	DateTime InvoiceDate,
	DateTime DueDate,
	string Currency,
	string Notes,
	CustomerResponse Customer,
	SenderResponse Sender,
	List<InvoiceLineItemResponse> LineItems,
	decimal Subtotal,
	decimal TaxRate,
	decimal TotalAmount
);
