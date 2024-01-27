using Blogg.Mapper.interfaces;
using Blogg.Models.AddDTO;
using Blogg.Models.DTOs;
using Blogg.Models.Entities;
using Blogg.Repository.interfaces;
using Blogg.Services.interfaces;

namespace Blogg.Services;
public class PostService : IPostService
{
    private readonly IMapper<Post, PostDTO> _postMapper;
    private readonly IMapper<Post, PostAddDTO> _postAddMapper;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<PostService> _logger;

    public PostService(IMapper<Post, PostDTO> postMapper,
                        IMapper<Post, PostAddDTO> postAddMapper,
                        IPostRepository postRepository,
                        IUserRepository userRepository,
                        ILogger<PostService> logger)
    {
        _postMapper = postMapper;
        _postAddMapper = postAddMapper;
        _postRepository = postRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<PostDTO?> AddAsync(PostAddDTO postAddDTO, int loginUserId)
    {
        try
        {
            var loginUser = await _userRepository.GetByIdAsync(loginUserId);

            var post = _postAddMapper.MapToModel(postAddDTO);

            post.UserId = loginUserId;

            var result = await _postRepository.AddAsync(post);

            _logger.LogInformation("Lagt til ny post: {post}", result);

            return result != null ? _postMapper.MapToDTO(result) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Feil under legging av posten");

            throw;
        }

    }

    public async Task<PostDTO?> DeleteByIdAsync(int deletePostId, int loginUserId)
    {
        var loginUser = await _userRepository.GetByIdAsync(loginUserId);

        if (loginUser == null) { return null; }

        var deletePost = await _postRepository.GetByIdAsync(deletePostId);

        if (deletePost == null)
        {
            _logger.LogError("Posten eksisterer ikke.");
            return null;
        }

        if (deletePost.UserId != loginUserId && !loginUser.IsAdminUser)
        {
            throw new UnauthorizedAccessException($"Bruker {loginUserId} har ikke tilgang til å slette");
        }

        await _postRepository.DeleteByIdAsync(deletePostId);


        return deletePost != null ? _postMapper.MapToDTO(deletePost) : null;
    }

    public async Task<ICollection<PostDTO>> GetAsync(int pageNr, int pageSize)
    {
        var res = await _postRepository.GetAsync(pageNr, pageSize);
        return res.Select(_postMapper.MapToDTO).ToList();
    }

    public async Task<PostDTO?> GetByIdAsync(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null) { return null; }
        return _postMapper.MapToDTO(post);
    }

    public async Task<ICollection<PostDTO>> GetPostsForUserAsync(int userId)
    {
        var res = await _postRepository.GetPostsForUserAsync(userId);

        // mapping
        var dtos = res.Select(res => _postMapper.MapToDTO(res)).ToList();
        return dtos;
    }

    public async Task<PostDTO?> UpdateAsync(int id, PostAddDTO postAddDTO,
                                            int loginUserId)
    {
        try
        {
            var loginUser = await _userRepository.GetByIdAsync(loginUserId);

            if (loginUser == null) { return null; }

            var existingPost = await _postRepository.GetByIdAsync(id);

            if (existingPost == null)
            {
                _logger.LogError("Fant ikke post med Id: {id}", id);
                return null;
            }

            if (existingPost.UserId != loginUserId && !loginUser.IsAdminUser)
            {
                throw new UnauthorizedAccessException($"Bruker {loginUserId} har ikke tilgang til å oppdatere post {id}");
            }

            var post = _postAddMapper.MapToModel(postAddDTO);
            post.Id = id;

            // Oppdater posten i databasen
            await _postRepository.UpdateAsync(id, post);

            // Henter den oppdaterte posten i databasen
            var updatedPost = await _postRepository.GetByIdAsync(id);


            if (updatedPost != null)
            {
                _logger.LogInformation("Post med ID '{id}' ble oppdatert.", id);
                return _postMapper.MapToDTO(updatedPost);
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Feil ved oppdatering av post: {id}", id);
            throw;
        }
    }
}
