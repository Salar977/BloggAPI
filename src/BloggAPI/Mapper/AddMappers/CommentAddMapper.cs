using BloggAPI.Mapper.interfaces;
using BloggAPI.Models.AddDTO;
using BloggAPI.Models.Entities;

namespace BloggAPI.Mapper.AddMappers;

public class CommentAddMapper : IMapper<Comment, CommentAddDTO>
{
    public CommentAddDTO MapToDTO(Comment model)
    {
        throw new NotImplementedException();
    }

    public Comment MapToModel(CommentAddDTO dto)
    {
        var now = DateTime.Now;
        return new Comment()
        {
            Content = dto.Kommentar,
            Created = now,
            Updated = now
        };
    }
}
