using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Common.API.Abstractions;
using Modules.Common.API.Extensions;
using Modules.Invoices.Features.Features.Shared.Routes;

namespace Modules.Invoices.Features.Features.Invoices.UpdateInvoice;

public sealed record UpdateInvoiceRequest(
	string InvoiceNumber,
	DateTime InvoiceDate,
	DateTime DueDate,
	string Currency,
	string Notes,
	Guid CustomerId,
	Guid SenderId,
	decimal Subtotal,
	decimal TaxRate,
	decimal TotalAmount);

public class UpdateInvoiceApiEndpoint : IApiEndpoint
{
	public void MapEndpoint(WebApplication app)
	{
		app.MapPut(RouteConsts.Update, Handle);
	}

	private static async Task<IResult> Handle(
		Guid id,
		[FromBody] UpdateInvoiceRequest request,
		IValidator<UpdateInvoiceRequest> validator,
		IUpdateInvoiceHandler handler,
		CancellationToken cancellationToken)
	{
		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
		{
			return Results.ValidationProblem(validationResult.ToDictionary());
		}

		var response = await handler.HandleAsync(id, request, cancellationToken);
		if (response.IsError)
		{
			return response.Errors.ToProblem();
		}

		return Results.Ok(response.Value);
	}
}
