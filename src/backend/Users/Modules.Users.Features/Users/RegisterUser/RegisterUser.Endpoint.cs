using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Common.API.Abstractions;
using Modules.Common.API.Extensions;
using Modules.Users.Features.Users.Shared.Routes;

namespace Modules.Users.Features.Users.RegisterUser;

public sealed record RegisterUserRequest(string Email, string Password, string? Role);

public class RegisterUserEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost(RouteConsts.Register, Handle);
    }

    private static async Task<IResult> Handle(
        [FromBody] RegisterUserRequest request,
        IValidator<RegisterUserRequest> validator,
        IRegisterUserHandler handler,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var response = await handler.HandleAsync(request, cancellationToken);
        if (response.IsError)
        {
            return response.Errors.ToProblem();
        }

        return Results.Created($"/api/users/{response.Value?.Id}", response.Value);
    }
}
