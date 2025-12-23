using AutoMapper;
using VoyagerTravelBlog.Application.Dtos.Post;
using VoyagerTravelBlog.Application.Exceptions;
using VoyagerTravelBlog.Application.Interfaces.Repositories;
using VoyagerTravelBlog.Application.Interfaces.Services;
using VoyagerTravelBlog.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace VoyagerTravelBlog.Application.Services
{
    public class PostService(IMapper mapper, IPostRepository postRepository, ILogger<PostService> logger) : IPostService
    {
        private readonly IMapper mapper = mapper;
        private readonly IPostRepository postRepository = postRepository;
        private readonly ILogger<PostService> logger = logger;

        public async Task<PostDto> AddPostAsync(CreatePostDto postToCreate)
        {
            try
            {
                var createdPost = await postRepository.CreateAsync(mapper.Map<Post>(postToCreate));

                return mapper.Map<PostDto>(createdPost);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding a post.");
                throw;
            }
        }

        public async Task<List<PostDto>> GetAllPostsAsync()
        {
            var posts = await postRepository.GetListAsync(p => true, includeProperties: "User, Medias");

            return mapper.Map<List<PostDto>>(posts);
        }

        public async Task<PostDto> GetPostByIdAsync(int id)
        {
            try
            {
                var post = await postRepository.GetAsync(p => p.Id == id, includeProperties: "User, Medias");

                return mapper.Map<PostDto>(post);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting a post by id {PostId}.", id);
                throw;
            }
        }

        public async Task<List<PostDto>> GetPostsByUserIdAsync(int userId)
        {
            var posts = await postRepository.GetListAsync(p => p.UserId == userId, includeProperties: "User, Medias");

            return mapper.Map<List<PostDto>>(posts);
        }

        public async Task RemovePostByIdAsync(int id)
        {
            try
            {
                var postToRemove = await postRepository.GetAsync(p => p.Id == id) ?? throw new NotFoundException(nameof(Post), id);

                await postRepository.RemoveAsync(postToRemove);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while removing a post by id {PostId}.", id);
                throw;
            }
        }

        public async Task RemovePostsByUserIdAsync(int userId)
        {
            try
            {
                var postsToRemove = await postRepository.GetListAsync(p => p.UserId == userId);

                if (!postsToRemove.Any())
                {
                    throw new NotFoundException(nameof(Post), nameof(User), userId);
                }

                await postRepository.RemoveListAsync(postsToRemove);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while removing posts by user id {UserId}.", userId);
                throw;
            }
        }

        public async Task<PostDto> UpdatePostAsync(UpdatePostDto postToUpdate)
        {
            try
            {
                var entityPost = await postRepository.GetAsync(p => p.Id == postToUpdate.Id) ?? throw new NotFoundException(nameof(Post), nameof(User), postToUpdate.Id);

                entityPost.Content = postToUpdate.Content;
                entityPost.Title = postToUpdate.Title;
                entityPost.Medias = mapper.Map<List<Media>>(postToUpdate.Medias);
                entityPost.UpdatedAt = DateTimeOffset.Now;

                await postRepository.UpdateAsync(entityPost);

                return mapper.Map<PostDto>(entityPost);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating a post with id {PostId}.", postToUpdate.Id);
                throw;
            }
        }
    }
}
