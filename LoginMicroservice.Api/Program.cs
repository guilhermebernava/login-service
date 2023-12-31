using LoginMicroservice.Infra.Injections;
using LoginMicroservice.Api.Injections;
using LoginMicroservice.Api.Profiles;
using LoginMicroservice.Api.Middlewares;
using LoginMicroservice.Api.RabbitMq;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddJWT(builder);
builder.Services.AddSingleton<IEmailSenderRabbitMq, EmailSenderRabbitMq>();
builder.Services.AddConfiguredSwagger();
builder.Services.AddCustomServices();
builder.Services.AddValidators();
builder.Services.AddAutoMapper(typeof(MappersProfile));
builder.Services.AddContextAndRepositories(builder.Configuration.GetConnectionString("Sql"));
builder.Services.AddRedisAndRepositories(builder.Configuration.GetConnectionString("Cache"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<GlobalErrorMiddleware>();
app.MapControllers();
app.Run();
