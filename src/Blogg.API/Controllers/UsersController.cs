using Microsoft.AspNetCore.Mvc;
using Blogg.Models.AddDTO;
using Blogg.Models.DTOs;
using Blogg.Services.interfaces;

namespace Blogg.Controllers;

[Route("api/v1/users")]
[ApiController]
public class UsersController : ControllerBase
{
	private readonly IUserService _userService;

    public UsersController( IUserService userService)
    {
		_userService = userService;
    }


    [HttpGet("GetUsers", Name = "GetAllUsersAsync")]
	public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsersAsync([FromRoute] int pageNr = 1,
																		   [FromRoute] int pageSize = 10)
	{
		return Ok(await _userService.GetAsync(pageNr, pageSize));
	}


    [HttpGet("{userId}", Name = "GetUserByIdAsync")]
	public async Task<ActionResult<UserDTO>> GetUserByIdAsync(int userId)
	{
		var userById = await _userService.GetByIdAsync(userId);
		return userById != null ? Ok(userById) : NotFound("Fant ikke brukeren med id: " + userId);
	}


	[HttpPost("register", Name = "RegisterUserAsync")]
	public async Task<ActionResult<UserDTO>> RegisterUserAsync(UserRegistrationDTO userRegDTO)
	{
		// 1. modelbinding har skjedd
		// 2. validering har skjedd
		if(!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		var userDTO = await _userService.RegisterAsync(userRegDTO);

		return userDTO != null
			? Ok(userDTO)
			: BadRequest("Klarte ikke registrere ny bruker");
	}


    [HttpPut("{userId}", Name = "UpdateUserAsync")]
    public async Task<ActionResult<UserDTO>> UpdateUserAsync(int userId,
                                                             UserRegistrationDTO userRegDTO)
    {
        int loginUserId = (int)this.HttpContext.Items["UserId"]!;

        var updatedUser = await _userService.UpdateAsync(userId, userRegDTO, loginUserId);

        return updatedUser != null ?
                   Ok(updatedUser) :
                   NotFound($"Klarte ikke å oppdatere bruker med ID: {userId}");
    }


    [HttpDelete("{userId}", Name = "DeleteUserAsync")]
	public async Task<ActionResult<UserDTO>> DeleteUserByIdAsync(int userId)
	{
		int loginUserId = (int)this.HttpContext.Items["UserId"]!;

		var del = await _userService.DeleteByIdAsync(userId, loginUserId);
		return del != null ?
				   Ok(del) :
				   NotFound($"Klarte ikke å slette bruker med ID: {userId}");
    }
}