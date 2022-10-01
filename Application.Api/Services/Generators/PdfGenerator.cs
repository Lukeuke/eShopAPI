using SelectPdf;

namespace Application.Api.Services.Generators;

public class PdfGenerator : IFileGenerator
{
    public object Generate(Models.Invoice invoice, string html)
    {
        var workingDirectory = Environment.CurrentDirectory;

        var pdf = new HtmlToPdf();

        var doc = pdf.ConvertHtmlString(html);

        var path = $"{workingDirectory}/invoice-{invoice.Number}.pdf";

        doc.Save(path);

        return path;
    }
}