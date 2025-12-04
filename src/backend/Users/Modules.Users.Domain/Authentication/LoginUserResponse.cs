namespace Modules.Users.Domain.Authentication;

public sealed record LoginUserResponse(string Token, string RefreshToken);