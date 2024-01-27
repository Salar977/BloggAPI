using Microsoft.EntityFrameworkCore;
using BloggAPI.Data;
using BloggAPI.Models.Entities;
using BloggAPI.Repository.interfaces;

namespace BloggAPI.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly StudentBloggDbContext _dbContext;

    public CommentRepository(StudentBloggDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Comment?> AddAsync(Comment comment)
    {
        var entry = await _dbContext.Comments.AddAsync(comment);
        await _dbContext.SaveChangesAsync();
        if (entry != null) { return entry.Entity; }

        return null;
    }

    public async Task<ICollection<Comment>> GetAsync(int pageNr, int pageSize)
    {
        return await _dbContext.Comments
            .OrderBy(x => x.Id)
            .Skip((pageNr - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        var comment = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);
        return comment is null ? null : comment;
    }

    public async Task<Comment?> DeleteByIdAsync(int id)
    {
        var comment = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);
        if (comment == null) return null;

        var entity = _dbContext.Comments.Remove(comment);
        await _dbContext.SaveChangesAsync();

        if (entity != null) return entity.Entity;

        return null;
    }

    public async Task<Comment?> UpdateAsync(int id, Comment comment)
    {
        var updateComment = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);
        if (updateComment == null) return null;

        updateComment.Content = string.IsNullOrEmpty(comment.Content) ? updateComment.Content : comment.Content;
        updateComment.Updated = DateTime.Now;

        await _dbContext.SaveChangesAsync();
        return updateComment;
    }

    public async Task<ICollection<Comment>> GetCommentsForPostAsync(int postId)
    {
        return await _dbContext.Comments
            .Where(x => x.PostId == postId)
            .ToListAsync();
    }
}
