using Microsoft.Extensions.DependencyInjection;

namespace Modules.Common.Infrastructure.Database;

public static class DatabaseMigrationExtensions
{
    public static async Task MigrateModuleDatabasesAsync(this IServiceScope scope,
        CancellationToken cancellationToken = default)
    {
        var migrators = scope.ServiceProvider.GetRequiredService<IEnumerable<IModuleDatabaseMigrator>>();

        foreach (var migrator in migrators)
        {
            await migrator.MigrateAsync(scope, cancellationToken);
        }
    }
}
