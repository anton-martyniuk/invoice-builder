namespace Modules.Invoices.Features.Features.Shared.Responses;

public sealed record CustomerResponse(
	Guid Id,
	string CompanyName,
	string CustomerName,
	string CustomerAddress,
	string PostalCode,
	string CustomerEmail,
	string CustomerTaxVatId
);
