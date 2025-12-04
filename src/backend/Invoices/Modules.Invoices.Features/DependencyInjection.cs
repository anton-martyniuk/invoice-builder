using FluentValidation;
using Microsoft.Extensions.Configuration;
using Modules.Common.Application;
using Modules.Common.Application.Extensions;
using Modules.Common.Domain.Events;
using Modules.Invoices.Features.Services;
using Modules.Invoices.Features.Tracing;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class InvoicesModuleRegistration
{
	public static string ActivityModuleName => InvoicesActivitySource.Instance.Name;

	public static IServiceCollection AddInvoicesModule(this IServiceCollection services, IConfiguration configuration)
	{
		return services
			.AddInvoicesModuleApi()
			.AddInvoicesInfrastructure(configuration);
	}

	private static IServiceCollection AddInvoicesModuleApi(this IServiceCollection services)
	{
		services.AddScoped<IEventPublisher, EventPublisher>();
		services.AddScoped<IInvoicePdfGenerator, InvoicePdfGenerator>();

		services.RegisterApiEndpointsFromAssemblyContaining(typeof(InvoicesModuleRegistration));

		services.RegisterHandlersFromAssemblyContaining(typeof(InvoicesModuleRegistration));

		services.AddValidatorsFromAssembly(typeof(InvoicesModuleRegistration).Assembly);

		return services;
	}
}
