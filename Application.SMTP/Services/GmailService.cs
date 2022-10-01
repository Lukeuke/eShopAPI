using System.Net;
using System.Net.Mail;
using Application.SMTP.Dtos;
using Newtonsoft.Json;

namespace Application.SMTP.Services;

public class GmailService :  ISmtpService
{
    public GmailService(RequestDto request)
    {
        SendEmail(request);
    }

    public GmailService(RequestDto request, string path)
    {
        SendEmailWithAttachment(request, path);
    }

    public GmailService()
    {
        
    }
    
    public (bool success, object content) SendEmail(RequestDto request)
    {
        var workingDirectory = Environment.CurrentDirectory;
        var serialized = File.ReadAllText(workingDirectory + @"/settings.json");
        var json = JsonConvert.DeserializeObject<SettingsDto>(serialized);

        var mail = json!.Username;
        var password = json.Password;
        
        using var mailMessage = new MailMessage
        {
            From = new MailAddress(mail),
            To = { request.ToAddress },
            Subject = request.Subject,
            Body = request.Body,
            IsBodyHtml = true
        };
        
        using var smtp = new SmtpClient
        {
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            EnableSsl = true,
            Host = "smtp.gmail.com",
            Port = 587,
            Credentials = new NetworkCredential(mail, password)
        };

        try
        {
            smtp.Send(mailMessage);
            return new ValueTuple<bool, object>(true, new { Message = "Mail was sent" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new ValueTuple<bool, object>(false, new { Message = "Mail was not sent" });
        }
    }

    public (bool success, object content) SendEmailWithAttachment(RequestDto request, string path)
    {
        var workingDirectory = Environment.CurrentDirectory;
        var serialized = File.ReadAllText(workingDirectory + @"/settings.json");
        var json = JsonConvert.DeserializeObject<SettingsDto>(serialized);

        var mail = json!.Username;
        var password = json.Password;

        Attachment att = null!;
        
        if (!string.IsNullOrEmpty(path))
        {
            att = new Attachment(path);
        }
        
        using var mailMessage = new MailMessage
        {
            From = new MailAddress(mail),
            To = { request.ToAddress },
            Subject = request.Subject,
            Body = request.Body,
            Attachments = { att },
            IsBodyHtml = true
        };
        
        using var smtp = new SmtpClient
        {
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            EnableSsl = true,
            Host = "smtp.gmail.com",
            Port = 587,
            Credentials = new NetworkCredential(mail, password)
        };

        try
        {
            smtp.Send(mailMessage);
            return new ValueTuple<bool, object>(true, new { Message = "Mail was sent" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new ValueTuple<bool, object>(false, new { Message = "Mail was not sent" });
        }
    }
}