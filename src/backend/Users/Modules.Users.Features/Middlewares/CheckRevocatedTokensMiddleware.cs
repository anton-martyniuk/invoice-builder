using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Modules.Users.Domain.Tokens;

namespace Modules.Users.Features.Middlewares;

/// <summary>
/// The middleware that checks if refresh tokens in webapi request where revocated
/// </summary>
public class CheckRevocatedTokensMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _memoryCache;

    /// <summary>
    /// Initializes a new instance of the class
    /// </summary>
    public CheckRevocatedTokensMiddleware(RequestDelegate next, IMemoryCache memoryCache)
    {
        _next = next;
        _memoryCache = memoryCache;
    }

    /// <summary>
    /// Invokes middleware
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        // Skip login and refresh URLs
        if (context.Request.Path.StartsWithSegments("/login", StringComparison.Ordinal)
            || context.Request.Path.StartsWithSegments("/refresh", StringComparison.Ordinal))
        {
            await _next(context);
            return;
        }

        // Skip users without a role
        var jwtId = context.User.FindFirst(JwtRegisteredClaimNames.Jti);
        var role = context.User.FindFirst(ClaimTypes.Role);
        if (jwtId is null || role is null)
        {
            await _next(context);
            return;
        }

        // Check if current JWT token of user is in revocation list
        var revocationType = _memoryCache.Get<RevocatedTokenType?>(jwtId.Value);
        if (revocationType.HasValue)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next(context);
    }
}
