using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Invoices.Domain.Entities;

namespace Modules.Invoices.Infrastructure.Database.Mapping;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
	public void Configure(EntityTypeBuilder<Invoice> entity)
	{
		entity.HasKey(x => x.Id);
		entity.HasIndex(x => x.InvoiceNumber);

		entity.Property(x => x.InvoiceNumber).IsRequired();
		entity.Property(x => x.InvoiceDate).IsRequired();
		entity.Property(x => x.DueDate).IsRequired();
		entity.Property(x => x.Currency).IsRequired();
		entity.Property(x => x.Notes).IsRequired();
		entity.Property(x => x.CustomerId).IsRequired();
		entity.Property(x => x.SenderId).IsRequired();
		entity.Property(x => x.Subtotal).IsRequired();
		entity.Property(x => x.TaxRate).IsRequired();
		entity.Property(x => x.TotalAmount).IsRequired();
		entity.Property(x => x.CreatedAt).IsRequired();

		entity.HasOne(x => x.Customer)
			.WithMany()
			.HasForeignKey(x => x.CustomerId)
			.OnDelete(DeleteBehavior.Restrict);

		entity.HasOne(x => x.Sender)
			.WithMany()
			.HasForeignKey(x => x.SenderId)
			.OnDelete(DeleteBehavior.Restrict);

		entity.HasMany(x => x.LineItems)
			.WithOne(x => x.Invoice)
			.HasForeignKey(x => x.InvoiceId);

		entity.Navigation(x => x.LineItems)
			.UsePropertyAccessMode(PropertyAccessMode.Field);
	}
}
