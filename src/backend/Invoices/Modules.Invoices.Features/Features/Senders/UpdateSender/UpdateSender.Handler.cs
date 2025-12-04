using Microsoft.EntityFrameworkCore;
using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Invoices.Features.Features.Senders.Shared.Errors;
using Modules.Invoices.Features.Features.Shared.Responses;
using Modules.Invoices.Infrastructure.Database;

namespace Modules.Invoices.Features.Features.Senders.UpdateSender;

internal interface IUpdateSenderHandler : IHandler
{
    Task<Result<SenderResponse>> HandleAsync(Guid id, UpdateSenderRequest request, CancellationToken cancellationToken);
}

internal sealed class UpdateSenderHandler(
    InvoicesDbContext dbContext)
    : IUpdateSenderHandler
{
    public async Task<Result<SenderResponse>> HandleAsync(Guid id, UpdateSenderRequest request, CancellationToken cancellationToken)
    {
        var sender = await dbContext.Senders.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (sender is null)
        {
            return SenderErrors.NotFound(id);
        }

        sender.Update(
            request.SenderCompanyName,
            request.SenderFullName,
            request.SenderAddress,
            request.SenderTaxVatId,
            request.BankDetails);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new SenderResponse(
            sender.Id,
            sender.SenderCompanyName,
            sender.SenderFullName,
            sender.SenderAddress,
            sender.SenderTaxVatId,
            sender.BankDetails);
    }
}
