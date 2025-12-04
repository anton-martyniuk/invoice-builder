using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Invoices.Features.Features.Shared.Errors;
using Modules.Invoices.Features.Features.Shared.Responses;
using Modules.Invoices.Infrastructure.Database;

namespace Modules.Invoices.Features.Features.Invoices.GetInvoiceById;

internal interface IGetInvoiceByIdHandler : IHandler
{
	Task<Result<InvoiceResponse>> HandleAsync(Guid id, CancellationToken cancellationToken);
}

internal sealed class GetInvoiceByIdHandler(
	InvoicesDbContext context)
	: IGetInvoiceByIdHandler
{
	public async Task<Result<InvoiceResponse>> HandleAsync(
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

		return invoice.MapToResponse();
	}
}
