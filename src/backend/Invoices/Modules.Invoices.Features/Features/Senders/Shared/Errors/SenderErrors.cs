using Modules.Common.Domain.Results;

namespace Modules.Invoices.Features.Features.Senders.Shared.Errors;

internal static class SenderErrors
{
    private const string ErrorCode = "Senders.Validation";

    public static Error NotFound(Guid senderId) =>
        Error.NotFound(ErrorCode, $"Sender with id '{senderId}' was not found");
}
