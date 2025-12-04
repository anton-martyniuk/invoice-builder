using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Common.API.Abstractions;
using Modules.Common.API.Extensions;
using Modules.Invoices.Features.Features.Customers.Shared.Routes;

namespace Modules.Invoices.Features.Features.Customers.GetCustomers;

public class GetCustomersApiEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet(RouteConsts.BaseRoute, Handle);
    }

    private static async Task<IResult> Handle(
        [FromQuery] int? offset,
        [FromQuery] int? limit,
        IGetCustomersHandler handler,
        CancellationToken cancellationToken)
    {
        var effectiveOffset = Math.Max(0, offset ?? 0);
        var requestedLimit = limit ?? 10;
        var effectiveLimit = Math.Max(1, Math.Min(100, requestedLimit));

        var response = await handler.HandleAsync(effectiveOffset, effectiveLimit, cancellationToken);
        if (response.IsError)
        {
            return response.Errors.ToProblem();
        }

        return Results.Ok(response.Value);
    }
}
