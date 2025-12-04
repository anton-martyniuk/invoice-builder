using System.Diagnostics;

namespace Modules.Invoices.Features.Tracing;

public static class InvoicesActivitySource
{
	internal static readonly ActivitySource Instance = new("invoices");
}
