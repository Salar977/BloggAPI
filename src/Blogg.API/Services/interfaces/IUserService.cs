using Blogg.Models.AddDTO;
using Blogg.Models.DTOs;

namespace Blogg.Services.interfaces;

public interface IUserService : IBaseService<UserDTO>
{
	Task<int> GetAuthenticatedIdAsync(string userName, string password);
    Task<UserDTO?> UpdateAsync(int id, UserRegistrationDTO userRegDTO, int loginUserId);
	Task<UserDTO?> RegisterAsync(UserRegistrationDTO userRegDTO);
}
