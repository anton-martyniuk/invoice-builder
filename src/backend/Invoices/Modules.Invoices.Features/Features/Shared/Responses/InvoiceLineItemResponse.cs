namespace Modules.Invoices.Features.Features.Shared.Responses;

public sealed record InvoiceLineItemResponse(
	Guid Id,
	string ItemName,
	decimal Quantity,
	decimal UnitPrice,
	decimal Total
);
