using Microsoft.EntityFrameworkCore;
using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Invoices.Features.Features.Customers.Shared.Errors;
using Modules.Invoices.Features.Features.Shared.Responses;
using Modules.Invoices.Infrastructure.Database;

namespace Modules.Invoices.Features.Features.Customers.UpdateCustomer;

internal interface IUpdateCustomerHandler : IHandler
{
    Task<Result<CustomerResponse>> HandleAsync(Guid id, UpdateCustomerRequest request, CancellationToken cancellationToken);
}

internal sealed class UpdateCustomerHandler(
    InvoicesDbContext dbContext)
    : IUpdateCustomerHandler
{
    public async Task<Result<CustomerResponse>> HandleAsync(Guid id, UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (customer is null)
        {
            return CustomerErrors.NotFound(id);
        }

        customer.Update(
            request.CompanyName,
            request.CustomerName,
            request.CustomerAddress,
            request.PostalCode,
            request.CustomerEmail,
            request.CustomerTaxVatId);

        await dbContext.SaveChangesAsync(cancellationToken);

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
