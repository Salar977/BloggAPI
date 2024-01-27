using Blogg.Models.DTOs;
using Blogg.Models.AddDTO;

namespace Blogg.Services.interfaces;

public interface IPostService : IBaseService<PostDTO>
{
    Task<PostDTO?> AddAsync(PostAddDTO postAddDTO, int loginUserId);
    Task<PostDTO?> UpdateAsync(int id, PostAddDTO postAddDTO, int loginUserId);
    Task<ICollection<PostDTO>> GetPostsForUserAsync(int userId);
}
