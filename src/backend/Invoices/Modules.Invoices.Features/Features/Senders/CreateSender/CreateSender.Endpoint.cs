using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Common.API.Abstractions;
using Modules.Common.API.Extensions;
using Modules.Invoices.Features.Features.Senders.Shared.Routes;

namespace Modules.Invoices.Features.Features.Senders.CreateSender;

public sealed record CreateSenderRequest(
    string SenderCompanyName,
    string SenderFullName,
    string SenderAddress,
    string SenderTaxVatId,
    string BankDetails);

public class CreateSenderApiEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost(RouteConsts.BaseRoute, Handle);
    }

    private static async Task<IResult> Handle(
        [FromBody] CreateSenderRequest request,
        IValidator<CreateSenderRequest> validator,
        ICreateSenderHandler handler,
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
