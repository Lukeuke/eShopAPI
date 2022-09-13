using System.Text.RegularExpressions;

namespace Application.SMTP.Helpers;

public static class EmailValidator
{
    public static bool Validate(string email)
    {
        var regex = new Regex(@"^[\w\.\-]+@[\w\-]+(\.\w+)+$",
            RegexOptions.CultureInvariant | RegexOptions.Singleline);
        
        return regex.IsMatch(email);
    }
}