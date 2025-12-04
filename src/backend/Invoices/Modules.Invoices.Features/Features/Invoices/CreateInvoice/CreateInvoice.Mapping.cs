using Modules.Invoices.Domain.Entities;
using Modules.Invoices.Features.Features.Shared.Responses;

namespace Modules.Invoices.Features.Features.Invoices.CreateInvoice;

internal static class CreateInvoiceMappingExtensions
{
	public static Invoice MapToInvoice(this CreateInvoiceRequest request)
		=> Invoice.Create(
			request.InvoiceNumber,
			request.InvoiceDate,
			request.DueDate,
			request.Currency,
			request.Notes,
			request.CustomerId,
			request.SenderId,
			request.Subtotal,
			request.TaxRate,
			request.TotalAmount,
			request.LineItems
				.Select(x => new InvoiceLineItem
				{
					Id = Guid.NewGuid(),
					ItemName = x.ItemName,
					Quantity = x.Quantity,
					UnitPrice = x.UnitPrice,
					Total = x.Total
				}).ToList()
		);

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
			invoice.LineItems
				.Select(x => new InvoiceLineItemResponse(x.Id, x.ItemName, x.Quantity, x.UnitPrice, x.Total))
				.ToList(),
			invoice.Subtotal,
			invoice.TaxRate,
			invoice.TotalAmount
		);
}
