using VoyagerTravelBlog.Application.Dtos.Post;

namespace VoyagerTravelBlog.Application.Interfaces.Services
{
    public interface IPostService
    {
        Task<PostDto> AddPostAsync(CreatePostDto postToCreate);
        Task<List<PostDto>> GetAllPostsAsync();
        Task<PostDto> GetPostByIdAsync(int id);
        Task<List<PostDto>> GetPostsByUserIdAsync(int userId);
        Task RemovePostByIdAsync(int id);
        Task RemovePostsByUserIdAsync(int userId);
        Task<PostDto> UpdatePostAsync(UpdatePostDto postToUpdate);
    }
}
