namespace Modules.Invoices.Features.Features.Shared.Responses;

public sealed record SenderResponse(
	Guid Id,
	string SenderCompanyName,
	string SenderFullName,
	string SenderAddress,
	string SenderTaxVatId,
	string BankDetails
);
