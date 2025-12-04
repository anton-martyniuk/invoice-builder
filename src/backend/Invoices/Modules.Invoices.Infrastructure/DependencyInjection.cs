using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Modules.Common.Infrastructure.Database;
using Modules.Invoices.Infrastructure.Database;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
	public static IServiceCollection AddInvoicesInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		var postgresConnectionString = configuration.GetConnectionString("Postgres");

		services.AddDbContext<InvoicesDbContext>(x => x
			.UseNpgsql(postgresConnectionString, npgsqlOptions =>
				npgsqlOptions.MigrationsHistoryTable(DbConsts.MigrationHistoryTableName, DbConsts.InvoicesSchemaName))
			.UseSnakeCaseNamingConvention()
		);

		services.AddScoped<IModuleDatabaseMigrator, InvoicesDatabaseMigrator>();

		return services;
	}
}
