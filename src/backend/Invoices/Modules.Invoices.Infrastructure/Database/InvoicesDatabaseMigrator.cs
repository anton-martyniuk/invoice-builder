using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modules.Common.Infrastructure.Database;

namespace Modules.Invoices.Infrastructure.Database;

public class InvoicesDatabaseMigrator : IModuleDatabaseMigrator
{
	public async Task MigrateAsync(
		IServiceScope scope,
		CancellationToken cancellationToken = default)
	{
		var dbContext = scope.ServiceProvider.GetRequiredService<InvoicesDbContext>();
		await dbContext.Database.MigrateAsync(cancellationToken);
	}
}
