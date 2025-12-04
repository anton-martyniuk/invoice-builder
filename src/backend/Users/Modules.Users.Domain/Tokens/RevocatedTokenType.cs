namespace Modules.Users.Domain.Tokens;

/// <summary>
/// Represents the types of token revocation
/// </summary>
public enum RevocatedTokenType
{
    /// <summary>
    /// Token is invalidated due to security concerns or user actions
    /// </summary>
    Invalidated,

    /// <summary>
    /// Token is invalidated due to role change
    /// </summary>
    RoleChanged
}