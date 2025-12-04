using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Common.API.Abstractions;
using Modules.Common.API.Extensions;
using Modules.Invoices.Features.Features.Shared.Requests;
using Modules.Invoices.Features.Features.Shared.Routes;

namespace Modules.Invoices.Features.Features.Invoices.CreateInvoice;

public sealed record CreateInvoiceRequest(
	string InvoiceNumber,
	DateTime InvoiceDate,
	DateTime DueDate,
	string Currency,
	string Notes,
	Guid CustomerId,
	Guid SenderId,
	decimal Subtotal,
	decimal TaxRate,
	decimal TotalAmount,
	List<InvoiceLineItemRequest> LineItems);

public class CreateInvoiceApiEndpoint : IApiEndpoint
{
	public void MapEndpoint(WebApplication app)
	{
		app.MapPost(RouteConsts.BaseRoute, Handle);
	}

	private static async Task<IResult> Handle(
		[FromBody] CreateInvoiceRequest request,
		IValidator<CreateInvoiceRequest> validator,
		ICreateInvoiceHandler handler,
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
