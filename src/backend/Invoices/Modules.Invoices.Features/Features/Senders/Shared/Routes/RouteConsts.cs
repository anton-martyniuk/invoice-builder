namespace Modules.Invoices.Features.Features.Senders.Shared.Routes;

internal static class RouteConsts
{
    internal const string BaseRoute = "/api/senders";

    internal const string GetById = $"{BaseRoute}/{{id:guid}}";

    internal const string Update = $"{BaseRoute}/{{id:guid}}";

    internal const string Delete = $"{BaseRoute}/{{id:guid}}";
}
