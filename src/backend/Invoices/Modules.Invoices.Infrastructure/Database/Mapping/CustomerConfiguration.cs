using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Invoices.Domain.Entities;

namespace Modules.Invoices.Infrastructure.Database.Mapping;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
	public void Configure(EntityTypeBuilder<Customer> entity)
	{
		entity.HasKey(x => x.Id);

		entity.Property(x => x.CompanyName).IsRequired();
		entity.Property(x => x.CustomerName).IsRequired();
		entity.Property(x => x.CustomerAddress).IsRequired();
		entity.Property(x => x.PostalCode).IsRequired();
		entity.Property(x => x.CustomerEmail).IsRequired();
		entity.Property(x => x.CustomerTaxVatId).IsRequired();
		entity.Property(x => x.CreatedAt).IsRequired();
	}
}
