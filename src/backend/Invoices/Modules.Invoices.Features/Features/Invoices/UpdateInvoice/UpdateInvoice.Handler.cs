using Microsoft.EntityFrameworkCore;
using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Invoices.Features.Features.Shared.Errors;
using Modules.Invoices.Features.Features.Shared.Responses;
using Modules.Invoices.Infrastructure.Database;

namespace Modules.Invoices.Features.Features.Invoices.UpdateInvoice;

internal interface IUpdateInvoiceHandler : IHandler
{
	Task<Result<InvoiceResponse>> HandleAsync(Guid id, UpdateInvoiceRequest request, CancellationToken cancellationToken);
}

internal sealed class UpdateInvoiceHandler(
    InvoicesDbContext context)
    : IUpdateInvoiceHandler
{
	public async Task<Result<InvoiceResponse>> HandleAsync(
		Guid id,
		UpdateInvoiceRequest request,
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

		invoice.Update(
			request.InvoiceDate,
			request.DueDate,
			request.Currency,
			request.Notes,
			request.CustomerId,
			request.SenderId,
			request.Subtotal,
			request.TaxRate,
			request.TotalAmount);

        await context.SaveChangesAsync(cancellationToken);

        return invoice.MapToResponse();
    }
}
