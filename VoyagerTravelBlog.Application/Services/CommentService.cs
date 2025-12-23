using AutoMapper;
using VoyagerTravelBlog.Application.Dtos.Comment;
using VoyagerTravelBlog.Application.Exceptions;
using VoyagerTravelBlog.Application.Interfaces.Repositories;
using VoyagerTravelBlog.Application.Interfaces.Services;
using VoyagerTravelBlog.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace VoyagerTravelBlog.Application.Services
{
    public class CommentService(IMapper mapper, ICommentRepository commentRepository, ILogger<CommentService> logger) : ICommentService
    {
        private readonly IMapper mapper = mapper;
        private readonly ICommentRepository commentRepository = commentRepository;
        private readonly ILogger<CommentService> logger = logger;

        public async Task<CommentDto> AddCommentAsync(CreateCommentDto commentToCreate)
        {
            try
            {
                var createdComment = await commentRepository.CreateAsync(mapper.Map<Comment>(commentToCreate));

                return mapper.Map<CommentDto>(createdComment);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding a comment.");
                throw;
            }
        }

        public async Task<CommentDto> GetCommentByIdAsync(int id)
        {
            try
            {
                var comment = await commentRepository.GetAsync(p => p.Id == id, includeProperties: "User");
                return mapper.Map<CommentDto>(comment);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting a comment by id {CommentId}.", id);
                throw;
            }
        }

        public async Task<List<CommentDto>> GetCommentByPostIdAsync(int postId)
        {
            var comments = await commentRepository.GetListAsync(c => c.PostId == postId, includeProperties: "User");
            return mapper.Map<List<CommentDto>>(comments);
        }

        public async Task RemoveCommentByIdAsync(int id)
        {
            try
            {
                var comment = await commentRepository.GetAsync(c => c.Id == id) ?? throw new NotFoundException(nameof(Comment), id);
                await commentRepository.RemoveAsync(comment);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while removing a comment by id {CommentId}.", id);
                throw;
            }
        }

        public async Task RemoveCommentsByPostIdAsync(int postId)
        {
            try
            {
                var comments = await commentRepository.GetListAsync(c => c.PostId == postId);
                if (!comments.Any())
                {
                    throw new NotFoundException(nameof(Comment), nameof(Post), postId);
                }
                await commentRepository.RemoveListAsync(comments);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while removing comments by post id {PostId}.", postId);
                throw;
            }
        }

        public async Task<CommentDto> UpdateCommentAsync(UpdateCommentDto commentToUpdate)
        {
            try
            {
                var commentEntity = await commentRepository.GetAsync(c => c.Id == commentToUpdate.Id) ?? throw new NotFoundException(nameof(Comment), commentToUpdate.Id);

                commentEntity.Content = commentToUpdate.Content;

                await commentRepository.UpdateAsync(commentEntity);

                return mapper.Map<CommentDto>(commentEntity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating comment with id {CommentId}.", commentToUpdate.Id);
                throw;
            }
        }
    }
}
