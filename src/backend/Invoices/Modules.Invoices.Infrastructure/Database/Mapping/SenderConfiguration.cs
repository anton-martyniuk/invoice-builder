using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Invoices.Domain.Entities;

namespace Modules.Invoices.Infrastructure.Database.Mapping;

public class SenderConfiguration : IEntityTypeConfiguration<Sender>
{
	public void Configure(EntityTypeBuilder<Sender> entity)
	{
		entity.HasKey(x => x.Id);

		entity.Property(x => x.SenderCompanyName).IsRequired();
		entity.Property(x => x.SenderFullName).IsRequired();
		entity.Property(x => x.SenderAddress).IsRequired();
		entity.Property(x => x.SenderTaxVatId).IsRequired();
		entity.Property(x => x.BankDetails).IsRequired();
		entity.Property(x => x.CreatedAt).IsRequired();
	}
}
