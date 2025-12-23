using VoyagerTravelBlog.Application;
using VoyagerTravelBlog.Application.Dtos.Auth;
using VoyagerTravelBlog.Application.Extensions;
using VoyagerTravelBlog.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace VoyagerTravelBlog.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IUserService userService, AuthenticatedUser authorizedUser, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IUserService userService = userService;
        private readonly AuthenticatedUser authorizedUser = authorizedUser;
        private readonly ILogger<AuthController> logger = logger;

        [HttpGet]
        [Route("is-logged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult IsLoggedAsync()
        {
            var oldRefreshToken = Request.Cookies["refreshToken"];
            var isLogged = !string.IsNullOrEmpty(oldRefreshToken) && authorizedUser.Id > 0;
            
            return Ok(isLogged);
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            var response = await userService.LoginUserAsync(loginDto);

            if (response.Success)
            {
                // Set new cookies
                Response.AddJwtCookie(response.AccessToken, response.AccessTokenExpirationMinutes);
                Response.AddRefreshTokenCookie(response.RefreshToken, response.RefreshTokenExpirationDate);

                return Ok();
            }

            return Unauthorized(response.Errors);
        }

        [HttpPost]
        [Route("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshAsync()
        {
            var oldRefreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(oldRefreshToken))
            {
                return Unauthorized("Refresh token is missing.");
            }

            var response = await userService.RefreshUserTokenAsync(authorizedUser.Id, oldRefreshToken);

            if (response.Success)
            {
                // Set new cookies
                Response.AddJwtCookie(response.AccessToken, response.AccessTokenExpirationMinutes);
                Response.AddRefreshTokenCookie(response.RefreshToken, response.RefreshTokenExpirationDate);

                return Ok();
            }

            return Unauthorized(response.Errors);
        }

        [HttpPost]
        [Route("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                await userService.RemoveRefreshToken(authorizedUser.Id);
                Response.CleanCookies(Request);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while logging out user {UserId}.", authorizedUser.Id);
                return BadRequest($"Unable to remove cookies. Error: {ex.Message}");
            }
        }
    }
}
