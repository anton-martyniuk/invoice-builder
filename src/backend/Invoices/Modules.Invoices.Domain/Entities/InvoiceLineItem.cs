namespace Modules.Invoices.Domain.Entities;

public class InvoiceLineItem
{
	public Guid Id { get; set; }

	public required string ItemName { get; set; }

	public required decimal Quantity { get; set; }

	public required decimal UnitPrice { get; set; }

	public required decimal Total { get; set; }

	public Guid InvoiceId { get; set; }

	public Invoice Invoice { get; set; } = null!;
}
