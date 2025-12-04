using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Modules.Common.API.Abstractions;
using Modules.Common.API.Extensions;
using Modules.Invoices.Features.Features.Shared.Routes;

namespace Modules.Invoices.Features.Features.Invoices.GetInvoiceById;

public class GetInvoiceByIdApiEndpoint : IApiEndpoint
{
	public void MapEndpoint(WebApplication app)
	{
		app.MapGet(RouteConsts.GetById, Handle);
	}

	private static async Task<IResult> Handle(
		Guid id,
		IGetInvoiceByIdHandler handler,
		CancellationToken cancellationToken)
	{
		var response = await handler.HandleAsync(id, cancellationToken);
		if (response.IsError)
		{
			return response.Errors.ToProblem();
		}

		return Results.Ok(response.Value);
	}
}
