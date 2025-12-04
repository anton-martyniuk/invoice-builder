using Microsoft.EntityFrameworkCore;
using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Invoices.Infrastructure.Database;

namespace Modules.Invoices.Features.Features.Senders.GetSenders;

public sealed record SenderListItem(
    Guid Id,
    string SenderCompanyName,
    string SenderFullName,
    string SenderTaxVatId);

public sealed record GetSendersResponse(
    List<SenderListItem> Items,
    int Offset,
    int Limit,
    int Total);

internal interface IGetSendersHandler : IHandler
{
    Task<Result<GetSendersResponse>> HandleAsync(int offset, int limit, CancellationToken cancellationToken);
}

internal sealed class GetSendersHandler(
    InvoicesDbContext dbContext)
    : IGetSendersHandler
{
    public async Task<Result<GetSendersResponse>> HandleAsync(int offset, int limit, CancellationToken cancellationToken)
    {
        var total = await dbContext.Senders.AsNoTracking().CountAsync(cancellationToken);

        var items = await dbContext.Senders
            .AsNoTracking()
            .OrderByDescending(s => s.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .Select(s => new SenderListItem(
                s.Id,
                s.SenderCompanyName,
                s.SenderFullName,
                s.SenderTaxVatId))
            .ToListAsync(cancellationToken);

        var response = new GetSendersResponse(items, offset, limit, total);
        return response;
    }
}
