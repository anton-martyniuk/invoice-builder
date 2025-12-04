using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Modules.Common.API.Abstractions;
using Modules.Common.API.Extensions;
using Modules.Invoices.Features.Features.Shared.Routes;

namespace Modules.Invoices.Features.Features.Invoices.DownloadInvoice;

public class DownloadInvoiceApiEndpoint : IApiEndpoint
{
	public void MapEndpoint(WebApplication app)
	{
		app.MapGet(RouteConsts.Download, Handle);
	}

	private static async Task<IResult> Handle(
		Guid id,
		IDownloadInvoiceHandler handler,
		CancellationToken cancellationToken)
	{
		var response = await handler.HandleAsync(id, cancellationToken);
		if (response.IsError)
		{
			return response.Errors.ToProblem();
		}

		var pdfData = response.Value!;

		return Results.File(
			fileContents: pdfData.FileBytes,
			contentType: "application/pdf",
			fileDownloadName: pdfData.FileName);
	}
}
