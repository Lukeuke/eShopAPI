using Application.SMTP.Dtos;

namespace Application.SMTP.Services;

public interface ISmtpService
{
    (bool success, object content) SendEmail(RequestDto request);
    (bool success, object content) SendEmailWithAttachment(RequestDto request, string path);
}