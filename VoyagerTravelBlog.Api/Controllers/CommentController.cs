using VoyagerTravelBlog.Application.Dtos.Comment;
using VoyagerTravelBlog.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VoyagerTravelBlog.Api.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController(ICommentService commentService) : ControllerBase
    {
        private readonly ICommentService commentService = commentService;

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCommentByIdAsync(int id)
        {
            var post = await commentService.GetCommentByIdAsync(id);
            return Ok(post);
        }

        [HttpGet]
        [Route("get-by-post/{postId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CommentDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCommentByPostIdAsync(int postId)
        {
            var comments = await commentService.GetCommentByPostIdAsync(postId);
            return Ok(comments);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Authorize]
        public async Task<IActionResult> UpdateCommentAsync(UpdateCommentDto commentToUpdate)
        {
            var result = await commentService.UpdateCommentAsync(commentToUpdate);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommentDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Authorize]
        public async Task<IActionResult> AddCommentAsync(CreateCommentDto commentToCreate)
        {
            var result = await commentService.AddCommentAsync(commentToCreate);
            return CreatedAtAction("GetCommentById", new { id = result.Id }, result);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize]
        public async Task<IActionResult> RemoveCommentByIdAsync(int id)
        {
            await commentService.RemoveCommentByIdAsync(id);
            return NoContent();
        }

        [HttpDelete]
        [Route("remove-by-post/{postId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize]
        public async Task<IActionResult> RemoveCommentsByPostIdAsync(int postId)
        {
            await commentService.RemoveCommentsByPostIdAsync(postId);
            return NoContent();
        }
    }
}
