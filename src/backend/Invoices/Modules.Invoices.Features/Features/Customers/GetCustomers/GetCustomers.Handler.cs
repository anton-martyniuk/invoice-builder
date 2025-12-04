using Microsoft.EntityFrameworkCore;
using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Invoices.Infrastructure.Database;

namespace Modules.Invoices.Features.Features.Customers.GetCustomers;

public sealed record CustomerListItem(
    Guid Id,
    string CompanyName,
    string CustomerName,
    string CustomerEmail);

public sealed record GetCustomersResponse(
    List<CustomerListItem> Items,
    int Offset,
    int Limit,
    int Total);

internal interface IGetCustomersHandler : IHandler
{
    Task<Result<GetCustomersResponse>> HandleAsync(int offset, int limit, CancellationToken cancellationToken);
}

internal sealed class GetCustomersHandler(
    InvoicesDbContext dbContext)
    : IGetCustomersHandler
{
    public async Task<Result<GetCustomersResponse>> HandleAsync(int offset, int limit, CancellationToken cancellationToken)
    {
        var total = await dbContext.Customers.AsNoTracking().CountAsync(cancellationToken);

        var items = await dbContext.Customers
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .Select(c => new CustomerListItem(
                c.Id,
                c.CompanyName,
                c.CustomerName,
                c.CustomerEmail))
            .ToListAsync(cancellationToken);

        var response = new GetCustomersResponse(items, offset, limit, total);
        return response;
    }
}
