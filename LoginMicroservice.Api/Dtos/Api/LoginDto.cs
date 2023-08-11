namespace LoginMicroservice.Api.Dtos;

public class LoginDto
{
    public LoginDto(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; private set; }
    public string Password { get; private set; }
}
