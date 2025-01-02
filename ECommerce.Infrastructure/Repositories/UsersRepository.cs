using Dapper;
using ECommerce.Core.Entities;
using ECommerce.Core.Repositories;
using ECommerce.Infrastructure.DbContext;

namespace ECommerce.Infrastructure.Repositories;

internal class UsersRepository : IUsersRepository
{
    private readonly DapperDbContext _dbContext;

    public UsersRepository(DapperDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppUser> Add(AppUser user)
    {
        user.Id = Guid.NewGuid();

        string query = "INSERT INTO public.\"Users\"" +
            "(\"Id\", \"Name\", \"Email\", \"Password\", \"Gender\")" +
            "VALUES (@Id, @Name, @Email, @Password, @Gender)";

        int rowsAffected = await _dbContext.DbConnection.ExecuteAsync(query, user);
        if(rowsAffected > 0)
            return user;

        return null!;
    }

    public async Task<AppUser?> Get(string email, string password)
    {
        string query = "SELECT * FROM public.\"Users\"" +
            "WHERE \"Email\" = @email AND \"Password\" = @password";

        return await _dbContext.DbConnection.QueryFirstOrDefaultAsync<AppUser>(query, new { email, password });
    }
}
