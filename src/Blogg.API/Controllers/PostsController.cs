using Microsoft.AspNetCore.Mvc;
using Blogg.Models.DTOs;
using Blogg.Services.interfaces;
using Blogg.Models.AddDTO;

namespace Blogg.Controllers;

[Route("api/v1/posts")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }


    [HttpGet("AllPosts", Name = "GetPostsAsync")]
    public async Task<ActionResult<IEnumerable<PostDTO>>> GetPostsAsync(int pageNr = 1, int pageSize = 10)
    {
        return Ok(await _postService.GetAsync(pageNr, pageSize));
    }

    [HttpGet("user/{userId}", Name = "GetPostsForUserAsync")]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetPostsForUserAsync([FromRoute] int userId)
    {
        return Ok(await _postService.GetPostsForUserAsync(userId));
    }


    [HttpGet("{postId}", Name = "GetPostByIdAsync")]
    public async Task<ActionResult<PostDTO>> GetPostAsync(int postId)
    {
        var postById = await _postService.GetByIdAsync(postId);
        return postById != null ?
                   Ok(postById) :
                   NotFound($"Fant ikke post med id: {postId}");
    }


    [HttpPost(Name = "AddPostAsync")]
    public async Task<ActionResult<PostDTO>> AddPostAsync(PostAddDTO postAddDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int loginUserId = (int)this.HttpContext.Items["UserId"]!;

        var post = await _postService.AddAsync(postAddDTO, loginUserId);

        return post != null
            ? Ok(post)
            : BadRequest("Klarte ikke lage ny post.");

    }


    [HttpPut("{postId}", Name = "UpdatePostsync")]
    public async Task<ActionResult<PostDTO>> UpdatePostAsync(int postId, PostAddDTO postAddDTO)
    {
        int loginUserId = (int)this.HttpContext.Items["UserId"]!;

        var updatedPost = await _postService.UpdateAsync(postId, postAddDTO, loginUserId);

        return updatedPost != null ?
                   Ok(updatedPost) :
                   NotFound($"Klarte ikke å oppdatere post med ID: {postId}");
    }


    [HttpDelete("{postId}", Name = "DeletePostByIdAsync")]
    public async Task<ActionResult<PostDTO>> DeletePostByIdAsync(int postId)
    {
        int loginUserId = (int)this.HttpContext.Items["UserId"]!;

        var del = await _postService.DeleteByIdAsync(postId, loginUserId);

        return del != null ?
                   Ok(del) :
                   NotFound($"Klarte ikke å slette post med ID: {postId}");
    }
}
