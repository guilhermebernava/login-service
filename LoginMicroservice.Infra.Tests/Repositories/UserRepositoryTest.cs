using LoginMicroservice.Domain.Entities;
using LoginMicroservice.Domain.Repositories;
using LoginMicroservice.Infra.Data;
using LoginMicroservice.Infra.Exceptions;
using LoginMicroservice.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LoginMicroservice.Infra.Tests.Repositories;

public class UserRepositoryTest
{
    public IUserRepository UserRepository = new UserRepository(new LoginContext(new DbContextOptionsBuilder<LoginContext>().UseInMemoryDatabase(databaseName: "UserTestDb").Options));

    [Fact]
    public async Task ItShouldCreateAnUser()
    {
        var created = await UserRepository.CreateAsync(new User("teste@teste.com", "abc123", false));
        Assert.True(created);
    }

    [Fact]
    public async Task ItShouldDeleteAnUser()
    {
        var user = new User("teste@teste.com", "abc123", false);
        var created = await UserRepository.CreateAsync(user);
        Assert.True(created);

        var deleted = await UserRepository.DeleteAsync(user.Id);
        Assert.True(deleted);
    }

    [Fact]
    public async Task ItShouldGetAnUserById()
    {
        var user = new User("teste@teste.com", "abc123", false);
        var created = await UserRepository.CreateAsync(user);
        Assert.True(created);

        var entity = await UserRepository.GetByIdAsync(user.Id);
        Assert.NotNull(entity);
    }

    [Fact]
    public async Task ItShouldNotGetAnUserById()
    {
        var user = new User("teste@teste.com", "abc123", false);
        var created = await UserRepository.CreateAsync(user);
        Assert.True(created);

        await Assert.ThrowsAsync<LoginContextException>(async () => await UserRepository.GetByIdAsync(Guid.Empty));
    }

    [Fact]
    public async Task ItShouldLoginAnUser()
    {
        var created = await UserRepository.CreateAsync(new User("teste@teste.com", "abc123", false));
        Assert.True(created);

        var logged = await UserRepository.LoginAsync("teste@teste.com", "abc123");
        Assert.NotNull(logged);
    }

    [Fact]
    public async Task ItShouldChangeUserTwoFactor()
    {
        var created = await UserRepository.CreateAsync(new User("teste@teste.com", "abc123", false));
        Assert.True(created);

        var changed = await UserRepository.ChangeTwoFactorAsync("teste@teste.com", "abc123");
        Assert.True(changed);
    }

    [Fact]
    public async Task ItShouldNotChangeUserTwoFactor()
    {
        var created = await UserRepository.CreateAsync(new User("teste@teste.com", "abc123", false));
        Assert.True(created);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await UserRepository.ChangeTwoFactorAsync("teste@teste.com", "abc1"));
    }

    [Fact]
    public async Task ItShouldCheckTwoFactor()
    {
        var created = await UserRepository.CreateAsync(new User("teste@teste.com", "abc123", false));
        Assert.True(created);

        var isTwoFactor = await UserRepository.IsTwoFactorLogin("teste@teste.com");
        Assert.False(isTwoFactor);
    }

    [Fact]
    public async Task ItShouldNotLoginAnUser()
    {
        var created = await UserRepository.CreateAsync(new User("teste@teste.com", "abc123", false));
        Assert.True(created);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await UserRepository.LoginAsync("teste@teste.com", "abc"));
    }

}
