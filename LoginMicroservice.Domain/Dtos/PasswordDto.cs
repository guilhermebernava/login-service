namespace LoginMicroservice.Domain.Dtos;

public class PasswordDto
{
    public PasswordDto(string passwordHash, byte[] salt)
    {
        PasswordHash = passwordHash;
        Salt = salt;
    }

    public string PasswordHash { get; private set; }
    public byte[] Salt { get; private set; }
}
