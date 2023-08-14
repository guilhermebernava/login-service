using LoginMicroservice.Api.Dtos.RabbitMq;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using IModel = RabbitMQ.Client.IModel;

namespace LoginMicroservice.Api.RabbitMq;

public class EmailSenderRabbitMq : IEmailSenderRabbitMq
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public EmailSenderRabbitMq(IConfiguration configuration)
    {
        _configuration = configuration;
        _connection = new ConnectionFactory() { HostName = _configuration["RabbitMqHost"], Port = int.Parse(_configuration["RabbitMqPort"]), UserName = _configuration["RabbitMqUser"], Password = _configuration["RabbitMqPassword"] }.CreateConnection();
        _channel = _connection.CreateModel();
    }
    public void Execute(string email, string code)
    {
        string mensagem = JsonSerializer.Serialize(new EmailDto(email, code));
        var body = Encoding.UTF8.GetBytes(mensagem);
        _channel.BasicPublish(exchange: "", routingKey: _configuration["EmailQueueName"], basicProperties: null, body: body);
    }
}
