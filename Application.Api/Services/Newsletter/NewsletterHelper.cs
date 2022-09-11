using System.Globalization;
using Application.Api.Models;

namespace Application.Api.Services.Newsletter;

public static class NewsletterHelper
{
    public static string GenerateSubject()
    {
        return "One of your products is available!";
    }
    
    public static string GenerateBody(Product product)
    {
        return $@"<h1> {product.Name} </h1>
                  <p> For {product.Price.ToString(CultureInfo.InvariantCulture)} </p>
                ";
    }
}