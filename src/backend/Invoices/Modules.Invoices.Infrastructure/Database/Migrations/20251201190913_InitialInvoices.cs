using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Invoices.Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class InitialInvoices : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "invoices");

        migrationBuilder.CreateTable(
            name: "customers",
            schema: "invoices",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                company_name = table.Column<string>(type: "text", nullable: false),
                customer_name = table.Column<string>(type: "text", nullable: false),
                customer_address = table.Column<string>(type: "text", nullable: false),
                postal_code = table.Column<string>(type: "text", nullable: false),
                customer_email = table.Column<string>(type: "text", nullable: false),
                customer_tax_vat_id = table.Column<string>(type: "text", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_customers", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "senders",
            schema: "invoices",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                sender_company_name = table.Column<string>(type: "text", nullable: false),
                sender_full_name = table.Column<string>(type: "text", nullable: false),
                sender_address = table.Column<string>(type: "text", nullable: false),
                sender_tax_vat_id = table.Column<string>(type: "text", nullable: false),
                bank_details = table.Column<string>(type: "text", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_senders", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "invoices",
            schema: "invoices",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                invoice_number = table.Column<string>(type: "text", nullable: false),
                invoice_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                currency = table.Column<string>(type: "text", nullable: false),
                notes = table.Column<string>(type: "text", nullable: false),
                customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                sender_id = table.Column<Guid>(type: "uuid", nullable: false),
                subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                tax_rate = table.Column<decimal>(type: "numeric", nullable: false),
                total_amount = table.Column<decimal>(type: "numeric", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_invoices", x => x.id);
                table.ForeignKey(
                    name: "fk_invoices_customers_customer_id",
                    column: x => x.customer_id,
                    principalSchema: "invoices",
                    principalTable: "customers",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_invoices_senders_sender_id",
                    column: x => x.sender_id,
                    principalSchema: "invoices",
                    principalTable: "senders",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "invoice_line_items",
            schema: "invoices",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                item_name = table.Column<string>(type: "text", nullable: false),
                quantity = table.Column<decimal>(type: "numeric", nullable: false),
                unit_price = table.Column<decimal>(type: "numeric", nullable: false),
                total = table.Column<decimal>(type: "numeric", nullable: false),
                invoice_id = table.Column<Guid>(type: "uuid", nullable: false),
                invoice_id1 = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_invoice_line_items", x => x.id);
                table.ForeignKey(
                    name: "fk_invoice_line_items_invoices_invoice_id",
                    column: x => x.invoice_id,
                    principalSchema: "invoices",
                    principalTable: "invoices",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_invoice_line_items_invoices_invoice_id1",
                    column: x => x.invoice_id1,
                    principalSchema: "invoices",
                    principalTable: "invoices",
                    principalColumn: "id");
            });

        migrationBuilder.CreateIndex(
            name: "ix_invoice_line_items_invoice_id",
            schema: "invoices",
            table: "invoice_line_items",
            column: "invoice_id");

        migrationBuilder.CreateIndex(
            name: "ix_invoice_line_items_invoice_id1",
            schema: "invoices",
            table: "invoice_line_items",
            column: "invoice_id1");

        migrationBuilder.CreateIndex(
            name: "ix_invoices_customer_id",
            schema: "invoices",
            table: "invoices",
            column: "customer_id");

        migrationBuilder.CreateIndex(
            name: "ix_invoices_invoice_number",
            schema: "invoices",
            table: "invoices",
            column: "invoice_number");

        migrationBuilder.CreateIndex(
            name: "ix_invoices_sender_id",
            schema: "invoices",
            table: "invoices",
            column: "sender_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "invoice_line_items",
            schema: "invoices");

        migrationBuilder.DropTable(
            name: "invoices",
            schema: "invoices");

        migrationBuilder.DropTable(
            name: "customers",
            schema: "invoices");

        migrationBuilder.DropTable(
            name: "senders",
            schema: "invoices");
    }
}
