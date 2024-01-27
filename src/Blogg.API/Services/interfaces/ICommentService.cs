using Blogg.Models.AddDTO;
using Blogg.Models.DTOs;

namespace Blogg.Services.interfaces;

public interface ICommentService : IBaseService<CommentDTO>
{
    Task<CommentDTO?> AddAsync(int postId, CommentAddDTO commentAddDTO, int loginUserId);
    Task<CommentDTO?> UpdateAsync(int id, CommentAddDTO commentDTO, int loginUserId);
    Task<ICollection<CommentDTO>> GetCommentsForPostAsync(int postId);
}
