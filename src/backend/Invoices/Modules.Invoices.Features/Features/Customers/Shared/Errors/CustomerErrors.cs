using Modules.Common.Domain.Results;

namespace Modules.Invoices.Features.Features.Customers.Shared.Errors;

internal static class CustomerErrors
{
    private const string ErrorCode = "Customers.Validation";

    public static Error NotFound(Guid customerId) =>
        Error.NotFound(ErrorCode, $"Customer with id '{customerId}' was not found");
}
