using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Invoices.Domain.Entities;

namespace Modules.Invoices.Infrastructure.Database.Mapping;

public class InvoiceLineItemConfiguration : IEntityTypeConfiguration<InvoiceLineItem>
{
	public void Configure(EntityTypeBuilder<InvoiceLineItem> entity)
	{
		entity.HasKey(x => x.Id);

		entity.Property(x => x.Id).ValueGeneratedOnAdd();

		entity.Property(x => x.ItemName).IsRequired();
		entity.Property(x => x.Quantity).IsRequired();
		entity.Property(x => x.UnitPrice).IsRequired();
		entity.Property(x => x.Total).IsRequired();
	}
}
