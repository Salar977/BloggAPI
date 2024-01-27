using Microsoft.AspNetCore.Mvc;
using BloggAPI.Models.AddDTO;
using BloggAPI.Models.DTOs;
using BloggAPI.Services.interfaces;

namespace BloggAPI.Controllers;

[Route("api/v1/comments")]
[ApiController]
public class CommentsController : Controller
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }


    [HttpGet("AllComments", Name = "GetCommentsAsync")]
    public async Task<ActionResult<IEnumerable<CommentDTO>>> GetCommentsAsync(int pageNr = 1, int pageSize = 10)
    {
        return Ok(await _commentService.GetAsync(pageNr, pageSize));
    }

    [HttpGet("post/{postId}", Name = "GetCommentsForPostAsync")]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetCommentsForPostAsync([FromRoute] int postId)
    {
        return Ok(await _commentService.GetCommentsForPostAsync(postId));
    }


    [HttpGet("{commentId}", Name = "GetCommentByIdAsync")]
    public async Task<ActionResult<CommentDTO>> GetCommentByIdAsync(int commentId)
    {
        var commentById = await _commentService.GetByIdAsync(commentId);
        return commentById != null ? Ok(commentById) : NotFound($"Fant ikke kommentar");
    }


    [HttpPost("{postId}", Name = "AddCommentAsync")]
    public async Task<ActionResult<CommentDTO>> AddCommentAsync([FromRoute] int postId, CommentAddDTO commentAddDTO)
    {
        int loginUserId = (int)this.HttpContext.Items["UserId"]!;

        var comment = await _commentService.AddAsync(postId, commentAddDTO, loginUserId);

        return comment != null ?
                       Ok(comment) :
                       NotFound($"Fant ikke post med ID: {postId}");
    }


    [HttpPut("{commentId}", Name = "UpdateCommentAsync")]
    public async Task<ActionResult<CommentDTO>> UpdateCommentAsync(int commentId, CommentAddDTO commentAddDTO)
    {
        int loginUserId = (int)this.HttpContext.Items["UserId"]!;

        var updatedComment = await _commentService.UpdateAsync(commentId, commentAddDTO, loginUserId);

        return updatedComment != null ?
                   Ok(updatedComment) :
                   NotFound($"Klarte ikke å oppdatere kommentar med ID: {commentId}");
    }


    [HttpDelete("{commentId}", Name = "DeleteByIdAsync")]
    public async Task<ActionResult<CommentDTO>> DeleteCommentByIdAsync(int commentId)
    {
        int loginUserId = (int)this.HttpContext.Items["UserId"]!;

        var del = await _commentService.DeleteByIdAsync(commentId, loginUserId);

        return del != null ?
                   Ok(del) :
                   NotFound($"Klarte ikke å slette kommentar med ID: {commentId}");
    }
}
