using Microsoft.EntityFrameworkCore;
using BloggAPI.Data;
using BloggAPI.Models.Entities;
using BloggAPI.Repository.interfaces;

namespace BloggAPI.Repository;

public class PostRepository : IPostRepository
{
    private readonly StudentBloggDbContext _dbContext;

    public PostRepository(StudentBloggDbContext context)
    {
        _dbContext = context;
    }

    public async Task<ICollection<Post>> GetAllPostsAsync(int pageNr, int pageSize)
    {
        return await _dbContext.Posts
            .OrderBy(x => x.Id)
            .Skip((pageNr - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    public async Task<Post?> AddAsync(Post post)
    {
        var entry = await _dbContext.Posts.AddAsync(post);
        await _dbContext.SaveChangesAsync();
        if (entry != null) { return entry.Entity; }

        return null;
    }

    public async Task<Post?> UpdateAsync(int id, Post post)
    {
        var updatePost = await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
        if (updatePost == null) return null;

        updatePost.Title = string.IsNullOrEmpty(post.Title) ? updatePost.Title : post.Title;
        updatePost.Content = string.IsNullOrEmpty(post.Content) ? updatePost.Content : post.Content;
        updatePost.Updated = DateTime.Now;

        await _dbContext.SaveChangesAsync();
        return updatePost;
    }

    public async Task<Post?> DeleteByIdAsync(int id)
    {
        var post = await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
        if (post == null) return null;

        var entity = _dbContext.Posts.Remove(post);
        await _dbContext.SaveChangesAsync();

        if (entity != null) return entity.Entity;

        return null;
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        var post = await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
        return post is null ? null : post;
    }

    public async Task<ICollection<Post>> GetAsync(int pageNr, int pageSize)
    {
        return await _dbContext.Posts
            .OrderBy(x => x.Id)
            .Skip((pageNr - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<ICollection<Post>> GetPostsForUserAsync(int userId)
    {
        return await _dbContext.Posts
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }
}
