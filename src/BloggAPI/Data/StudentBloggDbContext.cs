using Microsoft.EntityFrameworkCore;
using BloggAPI.Models.Entities;

namespace BloggAPI.Data;

public class StudentBloggDbContext : DbContext, IStudentBloggDbContext
{
	public StudentBloggDbContext(DbContextOptions<StudentBloggDbContext> options) : base(options)
    {
	}

	public DbSet<User> Users { get; set; }
	public DbSet<Post> Posts { get; set; }
	public DbSet<Comment> Comments { get; set; }
}
