using AutoMapper;
using LoginMicroservice.Api.Dtos;
using LoginMicroservice.Api.Services;
using LoginMicroservice.Api.Services.Interfaces;
using LoginMicroservice.Api.Validators;
using LoginMicroservice.Domain.Entities;
using LoginMicroservice.Infra.Data;
using LoginMicroservice.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LoginMicroservice.Api.Tests.Services;

public class CreateUserServicesTest
{

    private readonly ICreateUserServices _createUserServices;

    public CreateUserServicesTest()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserDto, User>();
        });
        var _mapper = configuration.CreateMapper();

        _createUserServices = new CreateUserServices(
            new UserRepository(
                new LoginContext(
                    new DbContextOptionsBuilder<LoginContext>().UseInMemoryDatabase(databaseName: "UserTestDb").Options)),
            _mapper,
            new UserDtoValidator());
    }

    [Fact]
    public async Task ItShouldCreateAnUser()
    {
        var result = await _createUserServices.Call(new UserDto("a@a.com", "123456", false));
        Assert.True(result);
    }

    [Fact]
    public async Task ItShouldThrowsValidationException()
    {
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _createUserServices.Call(new UserDto("aa.com", "13456", false)));
    }

    [Fact]
    public async Task ItShouldReturnFalseIfValueIsNull()
    {
        var result = await _createUserServices.Call();
        Assert.False(result);
    }
}
