using Microsoft.EntityFrameworkCore;
using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Invoices.Features.Features.Shared.Errors;
using Modules.Invoices.Features.Features.Shared.Responses;
using Modules.Invoices.Infrastructure.Database;

namespace Modules.Invoices.Features.Features.Invoices.CreateInvoice;

internal interface ICreateInvoiceHandler : IHandler
{
	Task<Result<InvoiceResponse>> HandleAsync(CreateInvoiceRequest request, CancellationToken cancellationToken);
}

internal sealed class CreateInvoiceHandler(
	InvoicesDbContext context)
	: ICreateInvoiceHandler
{
	public async Task<Result<InvoiceResponse>> HandleAsync(
		CreateInvoiceRequest request,
		CancellationToken cancellationToken)
	{
		var invoiceExists = await context.Invoices.AnyAsync(x => x.InvoiceNumber == request.InvoiceNumber, cancellationToken);
		if (invoiceExists)
		{
			return InvoiceErrors.AlreadyExists(request.InvoiceNumber);
		}

		var customerExists = await context.Customers.AnyAsync(x => x.Id == request.CustomerId, cancellationToken);
		if (!customerExists)
		{
			return InvoiceErrors.CustomerNotFound(request.CustomerId);
		}

		var senderExists = await context.Senders.AnyAsync(x => x.Id == request.SenderId, cancellationToken);
		if (!senderExists)
		{
			return InvoiceErrors.SenderNotFound(request.SenderId);
		}

		var invoice = request.MapToInvoice();

		await context.Invoices.AddAsync(invoice, cancellationToken);
		await context.SaveChangesAsync(cancellationToken);

		var createdInvoice = await context.Invoices
			.Include(x => x.Customer)
			.Include(x => x.Sender)
			.Include(x => x.Items)
			.FirstAsync(x => x.Id == invoice.Id, cancellationToken);

		return createdInvoice.MapToResponse();
	}
}
