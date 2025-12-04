using Microsoft.EntityFrameworkCore;
using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Invoices.Features.Features.Senders.Shared.Errors;
using Modules.Invoices.Infrastructure.Database;

namespace Modules.Invoices.Features.Features.Senders.DeleteSender;

internal interface IDeleteSenderHandler : IHandler
{
    Task<Result<Success>> HandleAsync(Guid id, CancellationToken cancellationToken);
}

internal sealed class DeleteSenderHandler(
    InvoicesDbContext dbContext)
    : IDeleteSenderHandler
{
    public async Task<Result<Success>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var sender = await dbContext.Senders.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (sender is null)
        {
            return SenderErrors.NotFound(id);
        }

        dbContext.Senders.Remove(sender);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
