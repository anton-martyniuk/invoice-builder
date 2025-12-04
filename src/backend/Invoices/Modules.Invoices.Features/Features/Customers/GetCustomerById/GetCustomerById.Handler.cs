using Microsoft.EntityFrameworkCore;
using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Invoices.Features.Features.Customers.Shared.Errors;
using Modules.Invoices.Features.Features.Shared.Responses;
using Modules.Invoices.Infrastructure.Database;

namespace Modules.Invoices.Features.Features.Customers.GetCustomerById;

internal interface IGetCustomerByIdHandler : IHandler
{
    Task<Result<CustomerResponse>> HandleAsync(Guid id, CancellationToken cancellationToken);
}

internal sealed class GetCustomerByIdHandler(
    InvoicesDbContext dbContext)
    : IGetCustomerByIdHandler
{
    public async Task<Result<CustomerResponse>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (customer is null)
        {
            return CustomerErrors.NotFound(id);
        }

        return new CustomerResponse(
            customer.Id,
            customer.CompanyName,
            customer.CustomerName,
            customer.CustomerAddress,
            customer.PostalCode,
            customer.CustomerEmail,
            customer.CustomerTaxVatId);
    }
}
