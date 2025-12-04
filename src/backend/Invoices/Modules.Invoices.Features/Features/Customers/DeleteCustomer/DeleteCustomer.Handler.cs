using Microsoft.EntityFrameworkCore;
using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Invoices.Features.Features.Customers.Shared.Errors;
using Modules.Invoices.Infrastructure.Database;

namespace Modules.Invoices.Features.Features.Customers.DeleteCustomer;

internal interface IDeleteCustomerHandler : IHandler
{
    Task<Result<Success>> HandleAsync(Guid id, CancellationToken cancellationToken);
}

internal sealed class DeleteCustomerHandler(
    InvoicesDbContext dbContext)
    : IDeleteCustomerHandler
{
    public async Task<Result<Success>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (customer is null)
        {
            return CustomerErrors.NotFound(id);
        }

        dbContext.Customers.Remove(customer);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
