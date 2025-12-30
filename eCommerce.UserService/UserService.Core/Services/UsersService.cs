using Mapster;
using eCommerce.UserService.Core.DTO;
using eCommerce.UserService.Core.Entities;
using eCommerce.UserService.Core.RepositoryContracts;
using eCommerce.UserService.Core.ServiceContracts;

namespace eCommerce.UserService.Core.Services;

internal class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }


    public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
    {
        ApplicationUser? user = await _usersRepository.GetUserByEmailAndPassword(loginRequest.Email, loginRequest.PasswordHash);

        if (user == null)
        {
            return null;
        }

        //return new AuthenticationResponse(
        //    user.UserID, 
        //    user.Email, 
        //    user.PersonName,
        //    user.Gender, 
        //    "dummy token", 
        //    Success: true
        //);
        return user.Adapt<AuthenticationResponse>() with { Success = true, Token = "token" };
    }


    public async Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
    {
        //Create a new ApplicationUser object from RegisterRequest
        // ApplicationUser user = new ApplicationUser()
        // {
        //     PersonName = registerRequest.PersonName,
        //     Email = registerRequest.Email,
        //     Password = registerRequest.Password,
        //     Gender = registerRequest.Gender.ToString()
        // };
        ApplicationUser user = registerRequest.Adapt<ApplicationUser>();

        // Add user to the repository
        ApplicationUser? registeredUser = await _usersRepository.AddUser(user);
        if (registeredUser == null)
        {
            return null;
        }

        //Return success response
        //return new AuthenticationResponse(
        //    registeredUser.UserID, 
        //    registeredUser.Email, 
        //    registeredUser.PersonName,
        //    registeredUser.Gender, 
        //    "token",
        //    Success: true
        //);
        return registeredUser.Adapt<AuthenticationResponse>() with { Success = true, Token = "token" };
    }

    public async Task<UserDTO> GetUserByUserID(Guid userID)
    {
        ApplicationUser? user = await _usersRepository.GetUserByUserID(userID);
        return _mapper.Map<UserDTO>(user);
    }

}