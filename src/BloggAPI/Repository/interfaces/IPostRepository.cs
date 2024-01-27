using BloggAPI.Models.Entities;

namespace BloggAPI.Repository.interfaces;

public interface IPostRepository : IRepository<Post>
{
    Task<Post?> AddAsync(Post post);
    Task<ICollection<Post>> GetPostsForUserAsync(int userId);
}