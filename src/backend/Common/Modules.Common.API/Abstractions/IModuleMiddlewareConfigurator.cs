using Microsoft.AspNetCore.Builder;

namespace Modules.Common.API.Abstractions;

public interface IModuleMiddlewareConfigurator
{
    IApplicationBuilder Configure(IApplicationBuilder app);
}
