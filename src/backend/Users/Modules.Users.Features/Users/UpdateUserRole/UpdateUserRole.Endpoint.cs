using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Common.API.Abstractions;
using Modules.Common.API.Extensions;
using Modules.Users.Domain.Policies;
using Modules.Users.Features.Users.Shared.Routes;

namespace Modules.Users.Features.Users.UpdateUserRole;

public sealed record UpdateUserRoleRequest(string NewRole);

public class UpdateUserRoleEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost(RouteConsts.UpdateUserRole, Handle)
            .RequireAuthorization(UserPolicyConsts.UpdatePolicy);
    }

    private static async Task<IResult> Handle(
        string userId,
        [FromBody] UpdateUserRoleRequest request,
        IValidator<UpdateUserRoleRequest> validator,
        IUpdateUserRoleHandler handler,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var response = await handler.HandleAsync(userId, request, cancellationToken);
        if (response.IsError)
        {
            return response.Errors.ToProblem();
        }

        return Results.NoContent();
    }
}
