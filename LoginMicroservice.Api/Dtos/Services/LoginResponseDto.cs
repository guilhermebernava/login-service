namespace LoginMicroservice.Api.Dtos;

public class LoginResponseDto
{
    public LoginResponseDto(string token)
    {
        Token = token;
        IsTwoFactorLogin = false;
    }

    public LoginResponseDto()
    {
        IsTwoFactorLogin = true;
    }

    public string? Token { get; private set; }
    public bool IsTwoFactorLogin { get; private set; }
}
