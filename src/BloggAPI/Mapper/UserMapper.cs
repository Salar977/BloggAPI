using BloggAPI.Mapper.interfaces;
using BloggAPI.Models.DTOs;
using BloggAPI.Models.Entities;

namespace BloggAPI.Mapper;

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
