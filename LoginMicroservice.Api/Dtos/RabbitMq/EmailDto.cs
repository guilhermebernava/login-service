namespace LoginMicroservice.Api.Dtos.RabbitMq;

public class EmailDto
{
    public EmailDto(string email, string code)
    {
        Email = email;
        Code = code;
    }

    public string Email { get; set; }
    public string Code { get; set; }
}
