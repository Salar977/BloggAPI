using Microsoft.EntityFrameworkCore;
using BloggAPI.Models.Entities;

namespace BloggAPI.Data;

public interface IStudentBloggDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
