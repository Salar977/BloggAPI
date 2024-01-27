using Blogg.Mapper.interfaces;
using Blogg.Models.AddDTO;
using Blogg.Models.Entities;

namespace Blogg.Mapper.AddMappers;

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
