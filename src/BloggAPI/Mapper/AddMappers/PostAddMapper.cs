using BloggAPI.Mapper.interfaces;
using BloggAPI.Models.AddDTO;
using BloggAPI.Models.Entities;

namespace BloggAPI.Mapper.AddMappers;

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
