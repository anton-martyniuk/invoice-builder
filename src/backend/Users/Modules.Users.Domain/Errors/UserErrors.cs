using Microsoft.AspNetCore.Identity;
using Modules.Common.Domain.Results;

namespace Modules.Users.Domain.Errors;

public static class UserErrors
{
    private const string ErrorPrefix = "Users";

    public static Error NotFound(string userId) =>
        Error.NotFound($"{ErrorPrefix}.{nameof(NotFound)}", $"User with ID {userId} not found");
    
    public static Error NotFoundByEmail(string email) =>
        Error.NotFound($"{ErrorPrefix}.{nameof(NotFound)}", $"User with email {email} not found");
    
    public static Error RegistrationFailed(IEnumerable<IdentityError> identityErrors) =>
        Error.NotFound($"{ErrorPrefix}.{nameof(RegistrationFailed)}", string.Join(", ", identityErrors.Select(e => e.Description)));
    
    public static Error UpdateFailed(IEnumerable<IdentityError> identityErrors) =>
        Error.NotFound($"{ErrorPrefix}.{nameof(UpdateFailed)}", string.Join(", ", identityErrors.Select(e => e.Description)));
    
    public static Error DeleteFailed(IEnumerable<IdentityError> identityErrors) =>
        Error.NotFound($"{ErrorPrefix}.{nameof(DeleteFailed)}", string.Join(", ", identityErrors.Select(e => e.Description)));
    
    public static Error RefreshFailed(IEnumerable<IdentityError> identityErrors) =>
        Error.NotFound($"{ErrorPrefix}.{nameof(RefreshFailed)}", string.Join(", ", identityErrors.Select(e => e.Description)));
    
    public static Error RoleNotFound(string roleName) =>
        Error.NotFound($"{ErrorPrefix}.{nameof(RoleNotFound)}", $"Role '{roleName}' not found");
    
    public static Error UpdateRoleFailed(IEnumerable<IdentityError> identityErrors) =>
        Error.NotFound($"{ErrorPrefix}.{nameof(UpdateRoleFailed)}", string.Join(", ", identityErrors.Select(e => e.Description)));

    public static Error InvalidCredentials() =>
        Error.Validation($"{ErrorPrefix}.{nameof(InvalidCredentials)}", "Invalid email or password");
    
    public static Error InvalidToken() =>
        Error.Validation($"{ErrorPrefix}.{nameof(InvalidToken)}", "Invalid token");
}
