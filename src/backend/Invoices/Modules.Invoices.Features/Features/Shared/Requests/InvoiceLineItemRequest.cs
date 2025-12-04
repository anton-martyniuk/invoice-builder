namespace Modules.Invoices.Features.Features.Shared.Requests;

public sealed record InvoiceLineItemRequest(
	string ItemName,
	decimal Quantity,
	decimal UnitPrice,
	decimal Total
);
