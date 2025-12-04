using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Modules.Common.API.Abstractions;
using Modules.Common.API.Extensions;
using Modules.Users.Domain.Policies;
using Modules.Users.Features.Users.Shared.Routes;

namespace Modules.Users.Features.Users.GetUserById;

public class GetUserByIdEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet(RouteConsts.GetById, Handle)
            .RequireAuthorization(UserPolicyConsts.ReadPolicy);
    }

    private static async Task<IResult> Handle(
        string userId,
        IGetUserByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.HandleAsync(userId, cancellationToken);
        if (response.IsError)
        {
            return response.Errors.ToProblem();
        }

        return Results.Ok(response.Value);
    }
}
