namespace Modules.Invoices.Features.Features.Shared.Routes;

internal static class RouteConsts
{
	internal const string BaseRoute = "/api/invoices";

	internal const string GetById = $"{BaseRoute}/{{id:guid}}";

	internal const string Update = $"{BaseRoute}/{{id:guid}}";

	internal const string Delete = $"{BaseRoute}/{{id:guid}}";

	internal const string Download = $"{BaseRoute}/{{id:guid}}/download";
}
