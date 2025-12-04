using System.Drawing;
using IronPdf.Rendering;
using IronPdf.Signing;
using Microsoft.Extensions.Logging;
using Modules.Invoices.Features.Features.Shared.Responses;
using Razor.Templating.Core;

namespace Modules.Invoices.Features.Services;

public interface IInvoicePdfGenerator
{
	Task<byte[]> GeneratePdfAsync(InvoiceResponse invoice);
}

public sealed class InvoicePdfGenerator(ILogger<InvoicePdfGenerator> logger) : IInvoicePdfGenerator
{
	private const string InvoiceTemplatePath = "~/Views/Invoice/InvoiceTemplate.cshtml";
	private const int DefaultMargin = 10;

	public async Task<byte[]> GeneratePdfAsync(InvoiceResponse invoice)
	{
		logger.LogInformation("Generating PDF for invoice {InvoiceNumber}", invoice.InvoiceNumber);

		try
		{
			var html = await RenderInvoiceHtmlAsync(invoice);
			var pdf = await ConvertHtmlToPdfAsync(html);

			AddSignatureToPdf(pdf, invoice.Sender.SenderFullName);

			var pdfBytes = pdf.BinaryData;

			logger.LogInformation("PDF generated successfully for invoice {InvoiceNumber}, size: {Size} bytes",
				invoice.InvoiceNumber, pdfBytes.Length);

			return pdfBytes;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error generating PDF for invoice {InvoiceNumber}", invoice.InvoiceNumber);
			throw;
		}
	}

	private async Task<string> RenderInvoiceHtmlAsync(InvoiceResponse invoice)
	{
		var html = await RazorTemplateEngine.RenderAsync(InvoiceTemplatePath, invoice);
		logger.LogDebug("HTML rendered successfully for invoice {InvoiceNumber}", invoice.InvoiceNumber);
		return html;
	}

	private static async Task<PdfDocument> ConvertHtmlToPdfAsync(string html)
	{
		var renderer = CreatePdfRenderer();

		var pdf = await renderer.RenderHtmlAsPdfAsync(html);
		pdf = pdf.ConvertToPdfUA();

		// var signature = new PdfSignature("Certificate.pfx", "123456")
		// {
		// 	SignatureDate = DateTime.Now,
		// 	SigningContact = "cert@company.com",
		// 	SigningLocation = "Chicago, USA",
		// 	SigningReason = "Contractual Agreement",
		// 	TimeStampUrl = "[http://timestamp.digicert.com](http://timestamp.digicert.com)",
		// 	TimestampHashAlgorithm = TimestampHashAlgorithms.SHA256,
		// 	SignatureImage = new PdfSignatureImage("assets/visual-signature.png", 0,
		// 		new Rectangle(350, 750, 200, 100))
		// };
		//
		// pdf.Sign(signature);

		return pdf;
	}

	private void AddSignatureToPdf(PdfDocument pdf, string senderFullName)
	{
		var signatureText = $"Digitally signed by: {senderFullName}";

		var stamp = new IronPdf.Editing.TextStamper
		{
			Text = signatureText,
			FontFamily = "Arial",
			FontSize = 20,
			IsBold = false,
			IsItalic = true,
			VerticalAlignment = IronPdf.Editing.VerticalAlignment.Bottom,
			HorizontalAlignment = IronPdf.Editing.HorizontalAlignment.Right,
			Opacity = 100,
			Rotation = 0
		};

		pdf.ApplyStamp(stamp);

		logger.LogDebug("Added signature to PDF: {SignatureText}", signatureText);
	}

	private static ChromePdfRenderer CreatePdfRenderer()
	{
		return new ChromePdfRenderer
		{
			RenderingOptions =
			{
				PaperSize = PdfPaperSize.A4,
				MarginTop = DefaultMargin,
				MarginBottom = DefaultMargin,
				MarginLeft = DefaultMargin,
				MarginRight = DefaultMargin,
				CssMediaType = PdfCssMediaType.Print,
				PrintHtmlBackgrounds = true,
				EnableJavaScript = true
			}
		};
	}
}
