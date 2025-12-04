using Microsoft.EntityFrameworkCore;
using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Invoices.Features.Features.Shared.Errors;
using Modules.Invoices.Infrastructure.Database;

namespace Modules.Invoices.Features.Features.Invoices.DeleteInvoice;

internal interface IDeleteInvoiceHandler : IHandler
{
	Task<Result<Success>> HandleAsync(Guid id, CancellationToken cancellationToken);
}

internal sealed class DeleteInvoiceHandler(
    InvoicesDbContext context)
    : IDeleteInvoiceHandler
{
    public async Task<Result<Success>> HandleAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var invoice = await context.Invoices
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (invoice is null)
        {
            return InvoiceErrors.NotFound(id);
        }

        context.Invoices.Remove(invoice);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
