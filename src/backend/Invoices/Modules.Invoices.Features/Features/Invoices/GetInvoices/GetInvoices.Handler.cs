using Microsoft.EntityFrameworkCore;
using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Invoices.Infrastructure.Database;

namespace Modules.Invoices.Features.Features.Invoices.GetInvoices;

public sealed record InvoiceListItem(
    Guid Id,
    string InvoiceNumber,
    DateTime InvoiceDate,
    DateTime DueDate,
    decimal TotalAmount);

public sealed record GetInvoicesResponse(
    List<InvoiceListItem> Items,
    int Offset,
    int Limit,
    int Total);

internal interface IGetInvoicesHandler : IHandler
{
    Task<Result<GetInvoicesResponse>> HandleAsync(int offset, int limit, CancellationToken cancellationToken);
}

internal sealed class GetInvoicesHandler(
    InvoicesDbContext dbContext)
    : IGetInvoicesHandler
{
    public async Task<Result<GetInvoicesResponse>> HandleAsync(int offset, int limit, CancellationToken cancellationToken)
    {
        var total = await dbContext.Invoices.AsNoTracking().CountAsync(cancellationToken);

        var items = await dbContext.Invoices
            .AsNoTracking()
            .OrderByDescending(i => i.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .Select(i => new InvoiceListItem(
                i.Id,
                i.InvoiceNumber,
                i.InvoiceDate,
                i.DueDate,
                i.TotalAmount))
            .ToListAsync(cancellationToken);

        var response = new GetInvoicesResponse(items, offset, limit, total);
        return response;
    }
}
