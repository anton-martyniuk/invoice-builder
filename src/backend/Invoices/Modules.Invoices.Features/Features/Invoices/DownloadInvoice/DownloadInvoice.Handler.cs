using Microsoft.EntityFrameworkCore;
using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Invoices.Features.Features.Shared.Errors;
using Modules.Invoices.Features.Services;
using Modules.Invoices.Infrastructure.Database;

namespace Modules.Invoices.Features.Features.Invoices.DownloadInvoice;

public sealed record InvoicePdfResponse(
	byte[] FileBytes,
	string FileName);

internal interface IDownloadInvoiceHandler : IHandler
{
	Task<Result<InvoicePdfResponse>> HandleAsync(Guid id, CancellationToken cancellationToken);
}

internal sealed class DownloadInvoiceHandler(
	InvoicesDbContext context,
	IInvoicePdfGenerator pdfGenerator)
	: IDownloadInvoiceHandler
{
	public async Task<Result<InvoicePdfResponse>> HandleAsync(
		Guid id,
		CancellationToken cancellationToken)
	{
		var invoice = await context.Invoices
			.Include(x => x.Customer)
			.Include(x => x.Sender)
			.Include(x => x.Items)
			.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

		if (invoice is null)
		{
			return InvoiceErrors.NotFound(id);
		}

		var invoiceResponse = invoice.MapToResponse();

		var pdfBytes = await pdfGenerator.GeneratePdfAsync(invoiceResponse);

		var fileName = $"Invoice_{invoice.InvoiceNumber}_{DateTime.UtcNow:yyyyMMdd}.pdf";

		return new InvoicePdfResponse(pdfBytes, fileName);
	}
}
