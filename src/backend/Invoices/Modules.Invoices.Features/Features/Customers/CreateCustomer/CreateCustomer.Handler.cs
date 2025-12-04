using Microsoft.EntityFrameworkCore;
using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Invoices.Domain.Entities;
using Modules.Invoices.Features.Features.Shared.Responses;
using Modules.Invoices.Infrastructure.Database;

namespace Modules.Invoices.Features.Features.Customers.CreateCustomer;

internal interface ICreateCustomerHandler : IHandler
{
    Task<Result<CustomerResponse>> HandleAsync(CreateCustomerRequest request, CancellationToken cancellationToken);
}

internal sealed class CreateCustomerHandler(
    InvoicesDbContext dbContext)
    : ICreateCustomerHandler
{
    public async Task<Result<CustomerResponse>> HandleAsync(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        // Optional: ensure uniqueness by email + company? For now, no unique validation specified.
        var entity = Customer.Create(
            request.CompanyName,
            request.CustomerName,
            request.CustomerAddress,
            request.PostalCode,
            request.CustomerEmail,
            request.CustomerTaxVatId);

        await dbContext.Customers.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CustomerResponse(
            entity.Id,
            entity.CompanyName,
            entity.CustomerName,
            entity.CustomerAddress,
            entity.PostalCode,
            entity.CustomerEmail,
            entity.CustomerTaxVatId);
    }
}
