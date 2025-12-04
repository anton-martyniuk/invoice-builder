using Microsoft.AspNetCore.Authorization;

namespace Modules.Common.Infrastructure.Policies;

/// <summary>
/// Represents a factory interface for creating authorization policies
/// specific to a module in the application.
/// </summary>
public interface IPolicyFactory
{
    /// <summary>
    /// Gets the name of the module associated with the policy factory.
    /// </summary>
    string ModuleName { get; }

    /// <summary>
    /// Retrieves a dictionary of policy configurations, where each policy is associated with an action
    /// to configure the corresponding <see cref="AuthorizationPolicyBuilder"/>.
    /// </summary>
    /// <returns>
    /// A dictionary containing policy names as keys and their corresponding configuration actions as values.
    /// </returns>
    Dictionary<string, Action<AuthorizationPolicyBuilder>> GetPolicies();
}
