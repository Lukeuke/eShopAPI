namespace Application.SMTP.Dtos;

public class RequestDto
{
    public string ToAddress { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    
    public override string ToString()
    {
        return $"{Subject} | {Body}";
    }
}