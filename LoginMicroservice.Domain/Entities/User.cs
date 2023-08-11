using LoginMicroservice.Domain.Helpers;

namespace LoginMicroservice.Domain.Entities;

public class User
{
    public User()
    {
        Id = Guid.NewGuid();
    }
    public User(string email, string password, bool twoFactor)
    {
        Id = Guid.NewGuid();
        Email = email;
        var passwordDto = PasswordHelper.GeneratePassword(password);

        PasswordHash = passwordDto.PasswordHash;
        PasswordSalt = passwordDto.Salt;
        TwoFactor = twoFactor;
    }

    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public bool TwoFactor { get; set; }
}
