using Microsoft.AspNetCore.Authorization;
using Modules.Common.Infrastructure.Policies;
using Modules.Users.Domain.Policies;

namespace Modules.Users.Infrastructure.Policies;

internal sealed class UsersPolicyFactory : IPolicyFactory
{
    public string ModuleName => "Users";

    public Dictionary<string, Action<AuthorizationPolicyBuilder>> GetPolicies()
    {
        return new Dictionary<string, Action<AuthorizationPolicyBuilder>>
        {
            [UserPolicyConsts.ReadPolicy] = policy => policy.RequireClaim(UserPolicyConsts.ReadPolicy),
            [UserPolicyConsts.CreatePolicy] = policy => policy.RequireClaim(UserPolicyConsts.CreatePolicy),
            [UserPolicyConsts.UpdatePolicy] = policy => policy.RequireClaim(UserPolicyConsts.UpdatePolicy),
            [UserPolicyConsts.DeletePolicy] = policy => policy.RequireClaim(UserPolicyConsts.DeletePolicy)
        };
    }
}
