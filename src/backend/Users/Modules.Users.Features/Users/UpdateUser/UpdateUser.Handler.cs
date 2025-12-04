using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Modules.Common.Domain.Handlers;
using Modules.Common.Domain.Results;
using Modules.Users.Domain.Errors;
using Modules.Users.Domain.Users;
using Modules.Users.Features.Users.Shared;

namespace Modules.Users.Features.Users.UpdateUser;

internal interface IUpdateUserHandler : IHandler
{
    Task<Result<UserResponse>> HandleAsync(string userId, UpdateUserRequest request, CancellationToken cancellationToken);
}

internal sealed class UpdateUserHandler(
    UserManager<User> userManager,
    ILogger<UpdateUserHandler> logger)
    : IUpdateUserHandler
{
    public async Task<Result<UserResponse>> HandleAsync(
        string userId,
        UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            logger.LogInformation("User with ID {UserId} not found", userId);
            return UserErrors.NotFound(userId);
        }

        user.Email = request.Email;
        user.UserName = request.Email;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            logger.LogError("Failed to update user: {@Errors}", result.Errors);
            return UserErrors.UpdateFailed(result.Errors);
        }

        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            var userRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, userRoles);

            await userManager.AddToRoleAsync(user, request.Role);
        }

        logger.LogInformation("Updated user with ID: {UserId}", userId);

        return new UserResponse(user.Id, user.Email);
    }
}
