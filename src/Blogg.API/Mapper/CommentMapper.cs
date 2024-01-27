using Blogg.Mapper.interfaces;
using Blogg.Models.DTOs;
using Blogg.Models.Entities;

namespace Blogg.Mapper;

public class CommentMapper : IMapper<Comment, CommentDTO>
{
    //TODO: create a CommentAddMapper
    public CommentDTO MapToDTO(Comment model)
    {
        return new CommentDTO(model.Id,
                              model.PostId,
                              model.UserId,
                              model.Content,
                              model.Created,
                              model.Updated);
    }

    public Comment MapToModel(CommentDTO dto)
    {
        var now = DateTime.Now;
        return new Comment
        {
            Id = dto.Id,
            PostId = dto.PostId,
            UserId = dto.UserId,
            Content = dto.CommentContent,
            Created = now,
            Updated = now
        };
    }
}
