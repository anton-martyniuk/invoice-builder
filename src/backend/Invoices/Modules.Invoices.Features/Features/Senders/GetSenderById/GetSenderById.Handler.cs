using Microsoft.EntityFrameworkCore;
using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Invoices.Features.Features.Senders.Shared.Errors;
using Modules.Invoices.Features.Features.Shared.Responses;
using Modules.Invoices.Infrastructure.Database;

namespace Modules.Invoices.Features.Features.Senders.GetSenderById;

internal interface IGetSenderByIdHandler : IHandler
{
    Task<Result<SenderResponse>> HandleAsync(Guid id, CancellationToken cancellationToken);
}

internal sealed class GetSenderByIdHandler(
    InvoicesDbContext dbContext)
    : IGetSenderByIdHandler
{
    public async Task<Result<SenderResponse>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var sender = await dbContext.Senders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (sender is null)
        {
            return SenderErrors.NotFound(id);
        }

        return new SenderResponse(
            sender.Id,
            sender.SenderCompanyName,
            sender.SenderFullName,
            sender.SenderAddress,
            sender.SenderTaxVatId,
            sender.BankDetails);
    }
}
