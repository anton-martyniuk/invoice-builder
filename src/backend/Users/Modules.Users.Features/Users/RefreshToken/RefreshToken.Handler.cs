using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Users.Domain.Authentication;

namespace Modules.Users.Features.Users.RefreshToken;

internal interface IRefreshTokenHandler : IHandler
{
    Task<Result<RefreshTokenResponse>> HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
}

internal sealed class RefreshTokenHandler(IClientAuthorizationService authorizationService)
    : IRefreshTokenHandler
{
    public async Task<Result<RefreshTokenResponse>> HandleAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authorizationService.RefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
        return result;
    }
}
