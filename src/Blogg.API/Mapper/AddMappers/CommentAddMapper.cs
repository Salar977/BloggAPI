using Blogg.Mapper.interfaces;
using Blogg.Models.AddDTO;
using Blogg.Models.Entities;

namespace Blogg.Mapper.AddMappers;

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
