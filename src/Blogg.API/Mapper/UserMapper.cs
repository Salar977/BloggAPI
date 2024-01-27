using Blogg.Mapper.interfaces;
using Blogg.Models.DTOs;
using Blogg.Models.Entities;

namespace Blogg.Mapper;

public class UserMapper : IMapper<User, UserDTO>
{
    public UserDTO MapToDTO(User model)
    {
        return new UserDTO(
                  model.Id,
                  model.UserName,
                  model.FirstName,
                  model.LastName,
                  model.Email,
                  model.Created,
                  model.Updated);
    }

    public User MapToModel(UserDTO dto)
    {
        var now = DateTime.Now;
        return new User()
        {
            Id = dto.Id,
            UserName = dto.UserName,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Created = now,
            Updated = now
        };
    }
}
