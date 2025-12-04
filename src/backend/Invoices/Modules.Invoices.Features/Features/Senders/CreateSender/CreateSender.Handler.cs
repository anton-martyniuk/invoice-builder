using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Invoices.Domain.Entities;
using Modules.Invoices.Features.Features.Shared.Responses;
using Modules.Invoices.Infrastructure.Database;

namespace Modules.Invoices.Features.Features.Senders.CreateSender;

internal interface ICreateSenderHandler : IHandler
{
    Task<Result<SenderResponse>> HandleAsync(CreateSenderRequest request, CancellationToken cancellationToken);
}

internal sealed class CreateSenderHandler(
    InvoicesDbContext dbContext)
    : ICreateSenderHandler
{
    public async Task<Result<SenderResponse>> HandleAsync(CreateSenderRequest request, CancellationToken cancellationToken)
    {
        var entity = Sender.Create(
            request.SenderCompanyName,
            request.SenderFullName,
            request.SenderAddress,
            request.SenderTaxVatId,
            request.BankDetails);

        await dbContext.Senders.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new SenderResponse(
            entity.Id,
            entity.SenderCompanyName,
            entity.SenderFullName,
            entity.SenderAddress,
            entity.SenderTaxVatId,
            entity.BankDetails);
    }
}
