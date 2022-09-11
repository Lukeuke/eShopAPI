namespace Application.SMTP.Dtos;

public class RequestDto
{
    public string ToAddress { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
    
    public override string ToString()
    {
        return $"{Subject} | {Body}";
    }
}