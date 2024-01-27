using Microsoft.EntityFrameworkCore;
using Blogg.Models.Entities;

namespace Blogg.Data;

public interface IStudentBloggDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
