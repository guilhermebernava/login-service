namespace LoginMicroservice.Api.Services.Interfaces;

public interface IServices<Return,Parameter>
{
    public Task<Return> Call(Parameter? value = default);
}
