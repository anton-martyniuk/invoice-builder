namespace Modules.Invoices.Domain.Entities;

public sealed class Sender
{
	public Guid Id { get; private init; }

	public string SenderCompanyName { get; private set; } = null!;

	public string SenderFullName { get; private set; } = null!;

	public string SenderAddress { get; private set; } = null!;

	public string SenderTaxVatId { get; private set; } = null!;

	public string BankDetails { get; private set; } = null!;

	public DateTime CreatedAt { get; private set; }

	public DateTime? UpdatedAt { get; private set; }

	private Sender()
	{
	}

	public static Sender Create(
		string senderCompanyName,
		string senderFullName,
		string senderAddress,
		string senderTaxVatId,
		string bankDetails)
	{
		return new Sender
		{
			Id = Guid.NewGuid(),
			SenderCompanyName = senderCompanyName,
			SenderFullName = senderFullName,
			SenderAddress = senderAddress,
			SenderTaxVatId = senderTaxVatId,
			BankDetails = bankDetails,
			CreatedAt = DateTime.UtcNow
		};
	}

	public void Update(
		string senderCompanyName,
		string senderFullName,
		string senderAddress,
		string senderTaxVatId,
		string bankDetails)
	{
		SenderCompanyName = senderCompanyName;
		SenderFullName = senderFullName;
		SenderAddress = senderAddress;
		SenderTaxVatId = senderTaxVatId;
		BankDetails = bankDetails;
		UpdatedAt = DateTime.UtcNow;
	}
}
