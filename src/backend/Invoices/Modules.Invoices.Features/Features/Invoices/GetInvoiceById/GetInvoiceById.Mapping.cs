using Modules.Invoices.Domain.Entities;
using Modules.Invoices.Features.Features.Shared.Responses;

namespace Modules.Invoices.Features.Features.Invoices.GetInvoiceById;

internal static class GetInvoiceByIdMappingExtensions
{
	public static InvoiceResponse MapToResponse(this Invoice invoice)
		=> new(
			invoice.Id,
			invoice.InvoiceNumber,
			invoice.InvoiceDate,
			invoice.DueDate,
			invoice.Currency,
			invoice.Notes,
			new CustomerResponse(
				invoice.Customer.Id,
				invoice.Customer.CompanyName,
				invoice.Customer.CustomerName,
				invoice.Customer.CustomerAddress,
				invoice.Customer.PostalCode,
				invoice.Customer.CustomerEmail,
				invoice.Customer.CustomerTaxVatId
			),
			new SenderResponse(
				invoice.Sender.Id,
				invoice.Sender.SenderCompanyName,
				invoice.Sender.SenderFullName,
				invoice.Sender.SenderAddress,
				invoice.Sender.SenderTaxVatId,
				invoice.Sender.BankDetails
			),
			invoice.Items
				.Select(x => new InvoiceLineItemResponse(
					x.Id,
					x.ItemName,
					x.Quantity,
					x.UnitPrice,
					x.Total
				))
				.ToList(),
			invoice.Subtotal,
			invoice.TaxRate,
			invoice.TotalAmount
		);
}
