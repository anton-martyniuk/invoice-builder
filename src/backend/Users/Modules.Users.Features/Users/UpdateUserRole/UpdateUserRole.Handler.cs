using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Users.Domain.Authentication;

namespace Modules.Users.Features.Users.UpdateUserRole;

internal interface IUpdateUserRoleHandler : IHandler
{
    Task<Result<Success>> HandleAsync(string userId, UpdateUserRoleRequest request, CancellationToken cancellationToken);
}

internal sealed class UpdateUserRoleHandler(
    IClientAuthorizationService authorizationService)
    : IUpdateUserRoleHandler
{
    public async Task<Result<Success>> HandleAsync(
        string userId,
        UpdateUserRoleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authorizationService.UpdateUserRoleAsync(userId, request.NewRole, cancellationToken);
        return result;
    }
}
