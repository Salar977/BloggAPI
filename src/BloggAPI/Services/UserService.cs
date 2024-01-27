using BloggAPI.Mapper.interfaces;
using BloggAPI.Models.AddDTO;
using BloggAPI.Models.DTOs;
using BloggAPI.Models.Entities;
using BloggAPI.Repository.interfaces;
using BloggAPI.Services.interfaces;

namespace BloggAPI.Services;
public class UserService : IUserService
{
    private readonly IMapper<User, UserDTO> _userMapper;
    private readonly IMapper<User, UserRegistrationDTO> _userRegMapper;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;


    public UserService(IMapper<User, UserDTO> userMapper,
		IMapper<User, UserRegistrationDTO> userRegMapper,
		IUserRepository userRepository,
        ILogger<UserService> logger)
        {
		_userMapper = userMapper;
        _userRegMapper = userRegMapper;
        _userRepository = userRepository;
        _logger = logger;
    }


    public async Task<UserDTO?> DeleteByIdAsync(int id, int loginUserId)
    {
        // sjekke har vi lov til å slette
        var loginUser = await _userRepository.GetByIdAsync(loginUserId) ??
            throw new UnauthorizedAccessException($"Bruker {loginUserId} har ikke tilgang til å slette");

        var deleteUser = await _userRepository.GetByIdAsync(id);

        if (deleteUser == null)
        {
            _logger.LogError("Brukeren eksisterer ikke.");
            return null;
        }

        if (id != loginUserId && !loginUser.IsAdminUser)
            throw new UnauthorizedAccessException($"Bruker {loginUserId} har ikke tilgang til å slette");



        await _userRepository.DeleteByIdAsync(id);

        return deleteUser != null ? _userMapper.MapToDTO(deleteUser) : null;
    }

    public async Task<ICollection<UserDTO>> GetAsync(int pageNr, int pageSize)
    {
        var res = await _userRepository.GetAsync(pageNr, pageSize);
        return res.Select(_userMapper.MapToDTO).ToList();
    }

    public async Task<int> GetAuthenticatedIdAsync(string userName, string password)
    {
        var usr = await _userRepository.GetUserByNameAsync(userName);

		if (usr == null) return 0;

		// prøver å verfisere passordet mot lagret hash-verdi
		if (BCrypt.Net.BCrypt.Verify(password, usr.HashedPassword))
		{
			return usr.Id;
		}
		return 0;
	}

    public async Task<UserDTO?> GetByIdAsync(int id)
    {
        var res = await _userRepository.GetByIdAsync(id);
        return res != null ? _userMapper.MapToDTO(res) : null;
    }

    public async Task<UserDTO?> RegisterAsync(UserRegistrationDTO userRegDTO)
    {
        try
        {
            var user = _userRegMapper.MapToModel(userRegDTO);

            // Lage salt og hashverdier
            user.Salt = BCrypt.Net.BCrypt.GenerateSalt();
            user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegDTO.Password, user.Salt);

            // Legg til bruker i databasen
            var res = await _userRepository.RegisterAsync(user);

            // Logg registreringen
            _logger?.LogInformation("Ny bruker registrert: {@user}", res);

            // Returner DTO av den registrerte brukeren
            return user != null ? _userMapper.MapToDTO(res!) : null;
        }
        catch (Exception ex)
        {
            // Logg feil under registreringen
            _logger.LogError(ex, "Feil under registrering av ny bruker");

            // Kast unntaket videre
            throw;
        }
    }

    public async Task<UserDTO?> UpdateAsync(int id, UserRegistrationDTO userRegDTO, int loginUserId)
    {
        try

        {
            var loginUser = await _userRepository.GetByIdAsync(loginUserId);
            if (loginUser == null)
            {
                _logger.LogError("Bruker {LoginUserId} har ikke tilgang til å oppdatere, bruker ikke funnet", loginUserId);
                throw new UnauthorizedAccessException($"Bruker {loginUserId} har ikke tilgang til å oppdatere");
            }

            var userToUpdate = await _userRepository.GetByIdAsync(id);
            if (userToUpdate == null) { return null; }

            if (id != loginUserId && !loginUser.IsAdminUser)
            {
                throw new UnauthorizedAccessException($"Bruker {loginUserId} har ikke tilgang til å oppdatere bruker {id}");
            }

            var user = _userRegMapper.MapToModel(userRegDTO);
            user.Id = id;

            // Oppdater brukeren i databasen
            await _userRepository.UpdateAsync(id, user);

            // Hent den oppdaterte brukeren fra databasen
            var updatedUser = await _userRepository.GetByIdAsync(id);

            if (updatedUser != null)
            {
                _logger?.LogInformation("Bruker med Id '{@id}' ble oppdatert.", id);

                // Returner de oppdaterte verdiene
                return _userMapper.MapToDTO(updatedUser);
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Feil ved oppdatering av bruker {@id}", id);
            throw;
        }
    }
}
