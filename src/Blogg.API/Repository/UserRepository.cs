using Microsoft.EntityFrameworkCore;
using Blogg.Data;
using Blogg.Models.Entities;
using Blogg.Repository.interfaces;

namespace Blogg.Repository;

public class UserRepository : IUserRepository
{
    private readonly StudentBloggDbContext _dbContext;

    public UserRepository(StudentBloggDbContext dbContext)
    {
		_dbContext = dbContext;
    }
    public async Task<User?> DeleteByIdAsync(int id)
    {
        var usr = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (usr == null) return null;

        var entity = _dbContext.Users.Remove(usr);
        await _dbContext.SaveChangesAsync();

        if (entity != null) return entity.Entity;

        return null;
    }

    public async Task<ICollection<User>> GetAsync(int pageNr, int pageSize)
    {
        return await _dbContext.Users
            .OrderBy(x => x.Id)
            .Skip((pageNr - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == id);

        return user is null ? null : user;
    }

    public async Task<User?> GetUserByNameAsync(string name)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.UserName!.Equals(name));
        return user is null ? null : user;
    }

    public async Task<User?> RegisterAsync(User user)
    {
        var entry = await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        if (entry != null) { return entry.Entity; }

        return null;
    }

    public async Task<User?> UpdateAsync(int id, User entity)
    {
        var updateUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (updateUser == null) return null;


        updateUser.UserName = string.IsNullOrEmpty(entity.UserName) ? updateUser.UserName : entity.UserName;
        updateUser.FirstName = string.IsNullOrEmpty(entity.FirstName) ? updateUser.FirstName : entity.FirstName;
        updateUser.LastName = string.IsNullOrEmpty(entity.LastName) ? updateUser.LastName : entity.LastName;
        updateUser.Email = string.IsNullOrEmpty(entity.Email) ? updateUser.Email : entity.Email;
        updateUser.Updated = DateTime.Now;

        await _dbContext.SaveChangesAsync();
        return updateUser;
    }
}
