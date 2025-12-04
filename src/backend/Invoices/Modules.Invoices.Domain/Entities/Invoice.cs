namespace Modules.Invoices.Domain.Entities;

public sealed class Invoice
{
	private readonly List<InvoiceLineItem> _lineItems = [];

	public Guid Id { get; private init; }

	public string InvoiceNumber { get; private set; } = null!;

	public DateTime InvoiceDate { get; private set; }

	public DateTime DueDate { get; private set; }

	public string Currency { get; private set; } = null!;

	public string Notes { get; private set; } = null!;

	public Guid CustomerId { get; private set; }

	public Customer Customer { get; private set; } = null!;

	public Guid SenderId { get; private set; }

	public Sender Sender { get; private set; } = null!;

	public decimal Subtotal { get; private set; }

	public decimal TaxRate { get; private set; }

	public decimal TotalAmount { get; private set; }

	public IReadOnlyList<InvoiceLineItem> LineItems => _lineItems.AsReadOnly();

	public IReadOnlyList<InvoiceLineItem> Items => _lineItems.AsReadOnly();

	public DateTime CreatedAt { get; private set; }

	public DateTime? UpdatedAt { get; private set; }

	private Invoice()
	{
	}

	public static Invoice Create(
		string invoiceNumber,
		DateTime invoiceDate,
		DateTime dueDate,
		string currency,
		string notes,
		Guid customerId,
		Guid senderId,
		decimal subtotal,
		decimal taxRate,
		decimal totalAmount,
		List<InvoiceLineItem> lineItems)
	{
		var invoice = new Invoice
		{
			Id = Guid.NewGuid(),
			InvoiceNumber = invoiceNumber,
			InvoiceDate = invoiceDate,
			DueDate = dueDate,
			Currency = currency,
			Notes = notes,
			CustomerId = customerId,
			SenderId = senderId,
			Subtotal = subtotal,
			TaxRate = taxRate,
			TotalAmount = totalAmount,
			CreatedAt = DateTime.UtcNow
		};

		invoice.AddLineItems(lineItems);

		return invoice;
	}

	public void Update(
		DateTime invoiceDate,
		DateTime dueDate,
		string currency,
		string notes,
		Guid customerId,
		Guid senderId,
		decimal subtotal,
		decimal taxRate,
		decimal totalAmount)
	{
		InvoiceDate = invoiceDate;
		DueDate = dueDate;
		Currency = currency;
		Notes = notes;
		CustomerId = customerId;
		SenderId = senderId;
		Subtotal = subtotal;
		TaxRate = taxRate;
		TotalAmount = totalAmount;
		UpdatedAt = DateTime.UtcNow;
	}

	public void AddLineItem(InvoiceLineItem item)
	{
		_lineItems.Add(item);
		UpdatedAt = DateTime.UtcNow;
	}

	public void AddLineItems(List<InvoiceLineItem> items)
	{
		_lineItems.AddRange(items);
		UpdatedAt = DateTime.UtcNow;
	}

	public void RemoveLineItem(InvoiceLineItem item)
	{
		_lineItems.Remove(item);
		UpdatedAt = DateTime.UtcNow;
	}
}
