using Microsoft.Extensions.DependencyInjection;

namespace Modules.Common.Infrastructure.Database;

public interface IModuleDatabaseMigrator
{
    Task MigrateAsync(IServiceScope scope, CancellationToken cancellationToken = default);
}
