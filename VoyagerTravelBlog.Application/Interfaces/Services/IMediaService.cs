using VoyagerTravelBlog.Application.Dtos.Media;

namespace VoyagerTravelBlog.Application.Interfaces.Services
{
    public interface IMediaService
    {
        Task<MediaDto> AddMediaAsync(CreateMediaDto mediaToCreate);
        Task<List<MediaDto>> AddMediasAsync(List<CreateMediaDto> mediasToCreate);
        Task<List<MediaDto>> GetAllMediasAsync();
        Task<List<MediaDto>> GetMediasByPostIdAsync(int postId);
        Task<MediaDto> GetMediaByIdAsync(int id);
        Task RemoveMediaByIdAsync(int id);
        Task RemoveMediasByPostIdAsync(int postId);
    }
}
