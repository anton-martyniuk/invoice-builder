using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Modules.Common.Infrastructure.Policies;

public class AuthorizationConfigureOptions(
    IEnumerable<IPolicyFactory> policyFactories,
    ILogger<AuthorizationConfigureOptions> logger)
    : IConfigureOptions<AuthorizationOptions>
{
    public void Configure(AuthorizationOptions options)
    {
        foreach (var factory in policyFactories)
        {
            logger.LogInformation("Configuring authorization policies for module: {ModuleName}", factory.ModuleName);
                
            var policies = factory.GetPolicies();
            
            foreach (var (policyName, policyBuilder) in policies)
            {
                options.AddPolicy(policyName, policyBuilder);
                logger.LogDebug("Added policy: {PolicyName}", policyName);
            }
        }
    }
}
