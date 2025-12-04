using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Users.Domain.Authentication;

namespace Modules.Users.Features.Users.LoginUser;

internal interface ILoginUserHandler : IHandler
{
    Task<Result<LoginUserResponse>> HandleAsync(LoginUserRequest request, CancellationToken cancellationToken);
}

internal sealed class LoginUserHandler(IClientAuthorizationService authorizationService)
    : ILoginUserHandler
{
    public async Task<Result<LoginUserResponse>> HandleAsync(
        LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authorizationService.LoginAsync(request.Email, request.Password, cancellationToken);
        return result;
    }
}
