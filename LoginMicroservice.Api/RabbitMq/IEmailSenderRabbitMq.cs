namespace LoginMicroservice.Api.RabbitMq;

public interface IEmailSenderRabbitMq
{
    public void Execute(string email, string code);
}
