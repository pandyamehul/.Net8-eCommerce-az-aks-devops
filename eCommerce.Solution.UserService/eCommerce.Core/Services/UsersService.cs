using Mapster;
using eCommerce.Core.DTO;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;

namespace eCommerce.Core.Services;

internal class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }


    public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
    {
        ApplicationUser? user = await _usersRepository.GetUserByEmailAndPassword(loginRequest.Email, loginRequest.Password);

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
}