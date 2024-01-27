using Blogg.Mapper.interfaces;
using Blogg.Models.AddDTO;
using Blogg.Models.Entities;

namespace Blogg.Mapper.AddMappers;

public class PostAddMapper : IMapper<Post, PostAddDTO>
{
    public PostAddDTO MapToDTO(Post model)
    {
        throw new NotImplementedException();
    }

    public Post MapToModel(PostAddDTO dto)
    {
        var now = DateTime.Now;
        return new Post()
        {
            Title = dto.Tittel,
            Content = dto.Innlegg,
            Created = now,
            Updated = now
        };
    }
}
