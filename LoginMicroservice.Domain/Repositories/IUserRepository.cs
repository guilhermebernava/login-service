using LoginMicroservice.Domain.Entities;

namespace LoginMicroservice.Domain.Repositories;

public interface IUserRepository
{
    public Task<bool> CreateAsync(User user, CancellationToken cancellationToken = default);
    public Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<User> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
    public Task<bool> ChangeTwoFactorAsync(string email, string password, CancellationToken cancellationToken = default);
    public Task<bool> IsTwoFactorLogin(string email, CancellationToken cancellationToken = default);
}
