using VoyagerTravelBlog.Application.Dtos;
using VoyagerTravelBlog.Application.Dtos.Auth;
using VoyagerTravelBlog.Application.Dtos.User;

namespace VoyagerTravelBlog.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> AddUserAsync(CreateUserDto userToCreate);
        Task<UserDto> GetUserByIdAsync(int id);
        Task RemoveUserByIdAsync(int id);
        Task UpdateUserAsync(UpdateUserDto userToUpdate);
        Task RemoveRefreshToken(int userId);
        Task<ValidateUserResponseDto> LoginUserAsync(LoginDto loginDto);
        Task<ValidateUserResponseDto> RefreshUserTokenAsync(int userId, string oldRefreshToken);
    }
}
