using Modules.Common.Domain.Results;

namespace Modules.Invoices.Features.Features.Shared.Errors;

internal static class InvoiceErrors
{
	private const string ErrorCode = "Invoices.Validation";

	public static Error NotFound(Guid invoiceId) =>
		Error.NotFound(ErrorCode, $"Invoice with id '{invoiceId}' was not found");

	public static Error AlreadyExists(string invoiceNumber) =>
		Error.Conflict(ErrorCode, $"Invoice with number '{invoiceNumber}' already exists");

	public static Error CustomerNotFound(Guid customerId) =>
		Error.NotFound(ErrorCode, $"Customer with id '{customerId}' was not found");

	public static Error SenderNotFound(Guid senderId) =>
		Error.NotFound(ErrorCode, $"Sender with id '{senderId}' was not found");
}
