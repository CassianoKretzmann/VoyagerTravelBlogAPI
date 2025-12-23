using AutoMapper;
using VoyagerTravelBlog.Application.Dtos.Media;
using VoyagerTravelBlog.Application.Exceptions;
using VoyagerTravelBlog.Application.Interfaces.Repositories;
using VoyagerTravelBlog.Application.Interfaces.Services;
using VoyagerTravelBlog.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace VoyagerTravelBlog.Application.Services
{
    public class MediaService(IMapper mapper, IMediaRepository mediaRepository, ILogger<MediaService> logger) : IMediaService
    {
        private readonly ILogger<MediaService> logger = logger;
        private readonly IMapper mapper = mapper;
        private readonly IMediaRepository mediaRepository = mediaRepository;

        public async Task<MediaDto> AddMediaAsync(CreateMediaDto mediaToCreate)
        {
            try
            {
                var createdMedia = await mediaRepository.CreateAsync(mapper.Map<Media>(mediaToCreate));

                return this.mapper.Map<MediaDto>(createdMedia);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding a media.");
                throw;
            }
        }

        public async Task<List<MediaDto>> AddMediasAsync(List<CreateMediaDto> mediasToCreate)
        {
            try
            {
                var createdMedias = await mediaRepository.CreateListAsync(this.mapper.Map<List<Media>>(mediasToCreate));

                return this.mapper.Map<List<MediaDto>>(createdMedias);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding medias.");
                throw;
            }
        }

        public async Task<List<MediaDto>> GetAllMediasAsync()
        {
            var medias = await mediaRepository.GetListAsync(m => true, includeProperties: "Post");

            return mapper.Map<List<MediaDto>>(medias);
        }

        public async Task<MediaDto> GetMediaByIdAsync(int id)
        {
            try
            {
                var media = await mediaRepository.GetAsync(m => m.Id == id, includeProperties: "Post");

                return mapper.Map<MediaDto>(media);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting a media by id {MediaId}.", id);
                throw;
            }
        }

        public async Task<List<MediaDto>> GetMediasByPostIdAsync(int postId)
        {
            var medias = await mediaRepository.GetListAsync(m => m.PostId == postId, includeProperties: "Post");

            return mapper.Map<List<MediaDto>>(medias);
        }

        public async Task RemoveMediaByIdAsync(int id)
        {
            try
            {
                var mediaToRemove = await mediaRepository.GetAsync(m => m.Id == id) ?? throw new NotFoundException(nameof(Media), id);

                await mediaRepository.RemoveAsync(mediaToRemove);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while removing a media by id {MediaId}.", id);
                throw;
            }
        }

        public async Task RemoveMediasByPostIdAsync(int postId)
        {
            try
            {
                var mediasToRemove = await mediaRepository.GetListAsync(m => m.PostId == postId);

                if (!mediasToRemove.Any())
                {
                    throw new NotFoundException(nameof(Media), nameof(Post), postId);
                }

                await mediaRepository.RemoveListAsync(mediasToRemove);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while removing medias by post id {PostId}.", postId);
                throw;
            }
        }
    }
}
