using Blogg.Models.Entities;

namespace Blogg.Repository.interfaces;

public interface IPostRepository : IRepository<Post>
{
    Task<Post?> AddAsync(Post post);
    Task<ICollection<Post>> GetPostsForUserAsync(int userId);
}