using BloggAPI.Mapper.interfaces;
using BloggAPI.Models.AddDTO;
using BloggAPI.Models.Entities;

namespace BloggAPI.Mapper.AddMappers;

public class UserRegMapper : IMapper<User, UserRegistrationDTO>
{
    public UserRegistrationDTO MapToDTO(User model)
    {
        throw new NotImplementedException();
    }

    public User MapToModel(UserRegistrationDTO dto)
    {
        var dtNow = DateTime.Now;
        return new User()
        {
            Email = dto.Email,
            UserName = dto.UserName,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            IsAdminUser = false,
            Created = dtNow,
            Updated = dtNow
        };
    }
}
