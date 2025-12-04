using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Common.API.Abstractions;
using Modules.Common.API.Extensions;
using Modules.Invoices.Features.Features.Customers.Shared.Routes;

namespace Modules.Invoices.Features.Features.Customers.CreateCustomer;

public sealed record CreateCustomerRequest(
    string CompanyName,
    string CustomerName,
    string CustomerAddress,
    string PostalCode,
    string CustomerEmail,
    string CustomerTaxVatId);

public class CreateCustomerApiEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost(RouteConsts.BaseRoute, Handle);
    }

    private static async Task<IResult> Handle(
        [FromBody] CreateCustomerRequest request,
        IValidator<CreateCustomerRequest> validator,
        ICreateCustomerHandler handler,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var response = await handler.HandleAsync(request, cancellationToken);
        if (response.IsError)
        {
            return response.Errors.ToProblem();
        }

        return Results.Ok(response.Value);
    }
}
