using LoginMicroservice.Domain.Entities;
using LoginMicroservice.Domain.Helpers;
using LoginMicroservice.Domain.Repositories;
using LoginMicroservice.Infra.Data;
using LoginMicroservice.Infra.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LoginMicroservice.Infra.Repositories;

public class UserRepository : IUserRepository
{
    public UserRepository(LoginContext dbContext)
    {
        DbContext = dbContext;
        DbSet = DbContext.Set<User>();
    }

    private LoginContext DbContext { get; set; }
    private DbSet<User> DbSet { get; set; }

    public async Task<bool> ChangeTwoFactorAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await LoginAsync(email, password, cancellationToken);
        user.TwoFactor = !user.TwoFactor;
        DbSet.Update(user);
        return await SaveAsync();
    }

    public async Task<bool> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            await DbSet.AddAsync(user, cancellationToken);
            return await SaveAsync();
        }
        catch (Exception e)
        {
            throw new LoginContextException(e.Message);
        }
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await GetByIdAsync(id, cancellationToken);
            DbSet.Remove(user);
            return await SaveAsync();
        }
        catch (Exception e)
        {
            throw new LoginContextException(e.Message);
        }
    }

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await DbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return user ?? throw new LoginContextException($"Not found any user with this ID - {id}");
        }
        catch (Exception e)
        {
            throw new LoginContextException(e.Message);
        }
    }

    public async Task<bool> IsTwoFactorLogin(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await DbSet.FirstOrDefaultAsync(x => x.Email == email, cancellationToken) ?? throw new LoginContextException("Could not login with this email or password");
            return user.TwoFactor;
        }
        catch (Exception e)
        {
            throw new LoginContextException(e.Message);
        }
    }

    public async Task<User> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {

        var user = await DbSet.FirstOrDefaultAsync(x => x.Email == email, cancellationToken) ?? throw new LoginContextException("Could not login with this email or password");
        if (!PasswordHelper.ValidatePassword(password, user.PasswordHash, user.PasswordSalt))
        {
            throw new UnauthorizedAccessException("Invalid EMAIL or PASSWORD");
        }

        return user;

    }

    private async Task<bool> SaveAsync()
    {
        return await DbContext.SaveChangesAsync() == 1;
    }
}
