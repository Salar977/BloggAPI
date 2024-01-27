using Blogg.Mapper.interfaces;
using Blogg.Models.DTOs;
using Blogg.Models.Entities;

namespace Blogg.Mapper
{
    public class PostMapper : IMapper<Post, PostDTO>
    {
        // Create a PostAddMapper
        public PostDTO MapToDTO(Post model)
        {
            return new PostDTO(
                  model.Id,
                  model.UserId,
                  model.Title,
                  model.Content,
                  model.Created,
                  model.Updated);
        }

        public Post MapToModel(PostDTO dto)
        {
            var now = DateTime.Now;
            return new Post
            {
                Id = dto.PostId,
                UserId = dto.UserId,
                Title = dto.Title,
                Content = dto.Content,
                Created = now,
                Updated = now,
            };
        }
    }
}
