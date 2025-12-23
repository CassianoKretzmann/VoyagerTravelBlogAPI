using VoyagerTravelBlog.Application.Dtos.Comment;

namespace VoyagerTravelBlog.Application.Interfaces.Services
{
    public interface ICommentService
    {
        Task<CommentDto> AddCommentAsync(CreateCommentDto commentToCreate);
        Task<CommentDto> GetCommentByIdAsync(int id);
        Task<List<CommentDto>> GetCommentByPostIdAsync(int postId);
        Task RemoveCommentByIdAsync(int id);
        Task RemoveCommentsByPostIdAsync(int postId);
        Task<CommentDto> UpdateCommentAsync(UpdateCommentDto commentToUpdate);
    }
}
