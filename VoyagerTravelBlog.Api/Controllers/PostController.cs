using VoyagerTravelBlog.Application.Dtos.Post;
using VoyagerTravelBlog.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VoyagerTravelBlog.Api.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController(IPostService postService) : ControllerBase
    {
        private readonly IPostService postService = postService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PostDto>))]
        public async Task<IActionResult> GetAllPostsAsync()
        {
            var posts = await postService.GetAllPostsAsync();
            return Ok(posts);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostByIdAsync(int id)
        {
            var post = await postService.GetPostByIdAsync(id);
            return Ok(post);
        }

        [HttpGet]
        [Route("get-by-user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostByUserIdAsync(int userId)
        {
            var posts = await postService.GetPostsByUserIdAsync(userId);
            return Ok(posts);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Authorize]
        public async Task<IActionResult> UpdatePostAsync(UpdatePostDto postToUpdate)
        {
            var result = await postService.UpdatePostAsync(postToUpdate);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PostDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Authorize]
        public async Task<IActionResult> AddPostAsync(CreatePostDto postToCreate)
        {
            var result = await postService.AddPostAsync(postToCreate);
            return CreatedAtAction("GetPostById", new { id = result.Id }, result);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize]
        public async Task<IActionResult> RemovePostByIdAsync(int id)
        {
            await postService.RemovePostByIdAsync(id);
            return NoContent();
        }

        [HttpDelete]
        [Route("remove-by-user/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize]
        public async Task<IActionResult> RemovePostByUserIdAsync(int userId)
        {
            await postService.RemovePostsByUserIdAsync(userId);
            return NoContent();
        }
    }
}
