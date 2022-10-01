namespace Application.Api.Services.Generators;

public interface IFileGenerator
{
    object Generate(Models.Invoice invoice, string html);
}