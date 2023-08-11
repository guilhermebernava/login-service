using AutoMapper;
using FluentValidation;
using LoginMicroservice.Api.Dtos;
using LoginMicroservice.Api.Services.Interfaces;
using LoginMicroservice.Domain.Entities;
using LoginMicroservice.Domain.Repositories;

namespace LoginMicroservice.Api.Services;

public class CreateUserServices : ICreateUserServices
{
    public CreateUserServices(IUserRepository userRepository, IMapper mapper, IValidator<UserDto> validator)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _validator = validator;
    }

    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<UserDto> _validator;

    public async Task<bool> Call(UserDto? value)
    {
        if (value == null)
        {
            return false;
        }

        var validations = _validator.Validate(value);

        if (!validations.IsValid)
        {
            throw new ValidationException(validations.Errors);
        }

        var entity = _mapper.Map<User>(value);
        return await _userRepository.CreateAsync(entity);
    }
}
