using Dapper;
using eCommerce.UserService.Core.DTO;
using eCommerce.UserService.Core.Entities;
using eCommerce.UserService.Core.RepositoryContracts;
using eCommerce.UserService.Infrastructure.DbContext;

namespace eCommerce.UserService.Infrastructure.Repositories;

internal class UsersRepository : IUsersRepository
{
    private readonly DapperDbContext _dbContext;

    public UsersRepository(DapperDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApplicationUser?> AddUser(ApplicationUser user)
    {
        //Generate a new unique user ID for the user
        user.UserID = Guid.NewGuid();

        // SQL Query to insert user data into the "Users" table.
        string query = "INSERT INTO public.\"users\"(\"userid\", \"email\", \"personname\", \"gender\", \"passwordhash\") VALUES(@UserID, @Email, @PersonName, @Gender, @PasswordHash)";
        int rowCountAffected = await _dbContext.DbConnection.ExecuteAsync(query, user);

        if (rowCountAffected > 0)
        {
            return user;
        }
        else
        {
            return null;
        }
    }

    public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? passwordhash)
    {
        // return new ApplicationUser()
        // {
        //     UserID = Guid.NewGuid(),
        //     Email = email,
        //     PasswordHash = password,
        //     PersonName = "Person name",
        //     Gender = GenderOptions.Male.ToString()
        // };

        //SQL query to select a user by Email and Password
        string query = "SELECT * FROM public.\"users\" WHERE \"email\"=@Email AND \"passwordhash\"=@PasswordHash";
        var parameters = new { Email = email, PasswordHash = passwordhash };

        ApplicationUser? user = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(query, parameters);

        return user;
    }

    public async Task<ApplicationUser?> GetUserByUserID(Guid? userID)
    {
        string query = "SELECT * FROM public.\"users\" WHERE \"userid\"=@UserID";
        var parameters = new { UserID = userID };

        ApplicationUser? user = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(query, parameters);

        return user;
    }
}
