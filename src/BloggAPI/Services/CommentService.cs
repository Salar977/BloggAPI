using BloggAPI.Mapper.interfaces;
using BloggAPI.Models.AddDTO;
using BloggAPI.Models.DTOs;
using BloggAPI.Models.Entities;
using BloggAPI.Repository.interfaces;
using BloggAPI.Services.interfaces;

namespace BloggAPI.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;
    private readonly IMapper<Comment, CommentDTO> _commentMapper;
    private readonly IMapper<Comment, CommentAddDTO> _commentAddMapper;
    private readonly ILogger<CommentService> _logger;

    public CommentService(ICommentRepository commentRepository,
                          IUserRepository userRepository,
                          IPostRepository postRepository,
                          IMapper<Comment, CommentDTO> commentMapper,
                          IMapper<Comment, CommentAddDTO> commentAddMapper,
                          ILogger<CommentService> logger)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _postRepository = postRepository;
        _commentMapper = commentMapper;
        _commentAddMapper = commentAddMapper;
        _logger = logger;
    }

    public async Task<CommentDTO?> AddAsync(int postId, CommentAddDTO commentAddDTO,
                                            int loginUserId)
    {
        try
        {
            var loginUser = await _userRepository.GetByIdAsync(loginUserId);
            var post = await _postRepository.GetByIdAsync(postId);
            var comment = _commentAddMapper.MapToModel(commentAddDTO);

            if(post == null) { return  null; }

            comment.PostId = post.Id;
            comment.UserId = loginUserId;

            var result = await _commentRepository.AddAsync(comment);

            _logger.LogInformation("Lagt til ny Kommentar: {result}", result);
            
            return result != null ? _commentMapper.MapToDTO(result) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Det skjedde noe feil med kommentaren");
            throw;
        }
        
    }

    public async Task<CommentDTO?> DeleteByIdAsync(int id, int loginUserId)
    {
        // sjekke har vi lov til å slette
        var loginUser = await _userRepository.GetByIdAsync(loginUserId);

        if(loginUser == null) { return null; }

        var deleteComment = await _commentRepository.GetByIdAsync(id);

        if (deleteComment == null)
        {
            _logger.LogError("Kommentaren eksisterer ikke.");
            return null;
        }

        if (deleteComment.UserId != loginUserId && !loginUser.IsAdminUser)
        {
            throw new UnauthorizedAccessException($"Bruker {loginUserId} har ikke tilgang til å slette kommentaren.");
        }

        

        await _commentRepository.DeleteByIdAsync(id);


        return deleteComment != null ? _commentMapper.MapToDTO(deleteComment) : null;
    }

    public async Task<ICollection<CommentDTO>> GetAsync(int pageNr, int pageSize)
    {
        var res = await _commentRepository.GetAsync(pageNr, pageSize);
        return res.Select(_commentMapper.MapToDTO).ToList();
    }

    public async Task<CommentDTO?> GetByIdAsync(int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null) { return null; }
        return _commentMapper.MapToDTO(comment);
    }

    public async Task<ICollection<CommentDTO>> GetCommentsForPostAsync(int postId)
    {
        var res = await _commentRepository.GetCommentsForPostAsync(postId);

        // mapping
        var dtos = res.Select(res => _commentMapper.MapToDTO(res)).ToList();
        return dtos;
    }

    public async Task<CommentDTO?> UpdateAsync(int id, CommentAddDTO commentAddDTO,
                                               int loginUserId)
    {
        try
        {
            var loginUser = await _userRepository.GetByIdAsync(loginUserId);
            if (loginUser == null) return null;

            var existingComment = await _commentRepository.GetByIdAsync(id);

            if (existingComment == null)
            {
                _logger.LogError("Fant ikke Kommentar '{id}'.", id);
                return null;
            }

            if(existingComment.UserId != loginUserId && !loginUser.IsAdminUser)
            {
                throw new UnauthorizedAccessException($"Bruker {loginUserId} har ikke tilgang til å oppdatere kommentar {id}.");
            }

            var comment = _commentAddMapper.MapToModel(commentAddDTO);
            comment.Id = id;

            // Oppdater kommentaren i databasen
            await _commentRepository.UpdateAsync(id, comment);

            // henter den oppdaterte posten i databasen
            var updatedComment = await _commentRepository.GetByIdAsync(id);


            if (updatedComment != null)
            {
                _logger.LogInformation("Kommentar med ID: '{Id}' ble oppdatert.", id);
                return _commentMapper.MapToDTO(updatedComment);
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Feil ved oppdatering av Kommentar '{Id}'", id);
            throw;
        }
    }
}
