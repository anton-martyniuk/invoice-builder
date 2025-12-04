namespace Modules.Invoices.Domain.Entities;

public sealed class Customer
{
	public Guid Id { get; private init; }

	public string CompanyName { get; private set; } = null!;

	public string CustomerName { get; private set; } = null!;

	public string CustomerAddress { get; private set; } = null!;

	public string PostalCode { get; private set; } = null!;

	public string CustomerEmail { get; private set; } = null!;

	public string CustomerTaxVatId { get; private set; } = null!;

	public DateTime CreatedAt { get; private set; }

	public DateTime? UpdatedAt { get; private set; }

	private Customer()
	{
	}

	public static Customer Create(
		string companyName,
		string customerName,
		string customerAddress,
		string postalCode,
		string customerEmail,
		string customerTaxVatId)
	{
		return new Customer
		{
			Id = Guid.NewGuid(),
			CompanyName = companyName,
			CustomerName = customerName,
			CustomerAddress = customerAddress,
			PostalCode = postalCode,
			CustomerEmail = customerEmail,
			CustomerTaxVatId = customerTaxVatId,
			CreatedAt = DateTime.UtcNow
		};
	}

	public void Update(
		string companyName,
		string customerName,
		string customerAddress,
		string postalCode,
		string customerEmail,
		string customerTaxVatId)
	{
		CompanyName = companyName;
		CustomerName = customerName;
		CustomerAddress = customerAddress;
		PostalCode = postalCode;
		CustomerEmail = customerEmail;
		CustomerTaxVatId = customerTaxVatId;
		UpdatedAt = DateTime.UtcNow;
	}
}
