namespace LoginMicroservice.Api.Dtos;

public class UserDto
{
    public UserDto(string email, string password, bool twoFactor)
    {
        Email = email;
        Password = password;
        TwoFactor = twoFactor;
    }

    public string Email { get; private set; }
    public string Password { get; private set; }
    public bool TwoFactor { get; set; }
}
