using AutoMapper;
using ECommerce.Core.DTOs;
using ECommerce.Core.Entities;
using ECommerce.Core.Repositories;
using ECommerce.Core.ServiceContracts;

namespace ECommerce.Core.Services;

internal class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;

    public UsersService(IUsersRepository usersRepository, IMapper mapper)
    {
        _usersRepository = usersRepository;
        _mapper = mapper;
    }

    public async Task<AuthResponseDTO?> Login(LoginDTO loginDTO)
    {
        var user = await _usersRepository.Get(loginDTO.Email, loginDTO.Password);

        return user is not null ? _mapper.Map<AuthResponseDTO>(user) with { Success = true, Token = "Token" } : null;
    }

    public async Task<AuthResponseDTO?> Register(RegisterDTO registerDTO)
    {
        // Create user
        var newUser = _mapper.Map<AppUser>(registerDTO);

        var addedUser = await _usersRepository.Add(newUser);
        return addedUser is not null ? _mapper.Map<AuthResponseDTO>(addedUser) with { Success = true, Token = "Token" } : null;
    }

    public async Task<UserDTO?> GetById(Guid id)
    {
        return _mapper.Map<UserDTO?>(await _usersRepository.GetById(id));
    }
}
