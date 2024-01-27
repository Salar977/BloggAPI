using BloggAPI.Models.AddDTO;
using BloggAPI.Models.DTOs;

namespace BloggAPI.Services.interfaces;

public interface ICommentService : IBaseService<CommentDTO>
{
    Task<CommentDTO?> AddAsync(int postId, CommentAddDTO commentAddDTO, int loginUserId);
    Task<CommentDTO?> UpdateAsync(int id, CommentAddDTO commentDTO, int loginUserId);
    Task<ICollection<CommentDTO>> GetCommentsForPostAsync(int postId);
}
