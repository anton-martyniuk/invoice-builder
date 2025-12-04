namespace Modules.Invoices.Features.Features.Customers.Shared.Routes;

internal static class RouteConsts
{
    internal const string BaseRoute = "/api/customers";

    internal const string GetById = $"{BaseRoute}/{{id:guid}}";

    internal const string Update = $"{BaseRoute}/{{id:guid}}";

    internal const string Delete = $"{BaseRoute}/{{id:guid}}";
}
