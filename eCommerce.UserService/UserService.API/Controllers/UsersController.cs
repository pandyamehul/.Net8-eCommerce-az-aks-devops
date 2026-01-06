using eCommerce.UserService.Core.DTO;
using eCommerce.UserService.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }


    //GET /api/Users/{userID}
    [HttpGet("{userID}")]
    public async Task<IActionResult> GetUserByUserID(Guid userID)
    {
        await Task.Delay(1000);
        // throw new NotImplementedException();

        if (userID == Guid.Empty)
        {
            return BadRequest("Invalid User ID");
        }

        UserDTO? response = await _usersService.GetUserByUserID(userID);

        if (response == null)
        {
            return NotFound(response);
        }

        return Ok(response);
    }
}