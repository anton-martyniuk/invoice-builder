using Microsoft.EntityFrameworkCore;
using Modules.Invoices.Domain.Entities;

namespace Modules.Invoices.Infrastructure.Database;

public class InvoicesDbContext(DbContextOptions<InvoicesDbContext> options) : DbContext(options)
{
	public DbSet<Invoice> Invoices { get; set; }
	public DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }
	public DbSet<Customer> Customers { get; set; }
	public DbSet<Sender> Senders { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.HasDefaultSchema(DbConsts.InvoicesSchemaName);

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(InvoicesDbContext).Assembly);
	}
}
