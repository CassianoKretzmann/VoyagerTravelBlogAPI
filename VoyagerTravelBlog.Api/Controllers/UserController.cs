using VoyagerTravelBlog.Application;
using VoyagerTravelBlog.Application.Dtos.User;
using VoyagerTravelBlog.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VoyagerTravelBlog.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController(IUserService userService, AuthenticatedUser authorizedUser) : ControllerBase
    {
        private readonly IUserService userService = userService;
        private readonly AuthenticatedUser authorizedUser = authorizedUser;

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            var user = await userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpGet]
        [Route("profile")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetUserProfileAsync()
        {
            var user = await userService.GetUserByIdAsync(authorizedUser.Id);
            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddUserAsync(CreateUserDto userToCreate)
        {
            var result = await userService.AddUserAsync(userToCreate);
            return CreatedAtAction("GetUserById", new { id = result.Id }, result);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize]
        public async Task<IActionResult> RemoveUserByIdAsync(int id) 
        {
            await userService.RemoveUserByIdAsync(id);
            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserDto userToUpdate)
        {
            await userService.UpdateUserAsync(userToUpdate);
            return NoContent();
        }
    }
}
