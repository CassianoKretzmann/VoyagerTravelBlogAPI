using AutoMapper;
using VoyagerTravelBlog.Application.Dtos.Auth;
using VoyagerTravelBlog.Application.Dtos.User;
using VoyagerTravelBlog.Application.Exceptions;
using VoyagerTravelBlog.Application.Helpers;
using VoyagerTravelBlog.Application.Interfaces.Repositories;
using VoyagerTravelBlog.Application.Interfaces.Services;
using VoyagerTravelBlog.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace VoyagerTravelBlog.Application.Services
{
    public partial class UserService(
        IJwtTokenService jwtTokenService,
        ILogger<UserService> logger,
        IMapper mapper,
        IUserRepository userRepository) : IUserService
    {
        private readonly IJwtTokenService jwtTokenService = jwtTokenService;
        private readonly ILogger<UserService> logger = logger;
        private readonly IMapper mapper = mapper;
        private readonly IUserRepository userRepository = userRepository;

        public async Task<UserDto> AddUserAsync(CreateUserDto userToCreate)
        {
            try
            {
                ValidateUsername(userToCreate.Username);
                ValidatePassword(userToCreate.Password);

                var createdUser = await userRepository.CreateAsync(mapper.Map<User>(userToCreate));

                return mapper.Map<UserDto>(createdUser);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding a user.");
                throw;
            }
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await userRepository.GetAsync(u => u.Id == id);

                return mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting a user by id {UserId}.", id);
                throw;
            }
        }

        public async Task RemoveUserByIdAsync(int id)
        {
            try
            {
                var userToRemove = await userRepository.GetAsync(u => u.Id == id) ?? throw new NotFoundException(nameof(User), id); ;

                await userRepository.RemoveAsync(userToRemove);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while removing a user by id {UserId}.", id);
                throw;
            }
        }

        public async Task UpdateUserAsync(UpdateUserDto userToUpdate)
        {
            try
            {
                var user = await userRepository.GetAsync(u => u.Id == userToUpdate.Id) ?? throw new NotFoundException(nameof(User), userToUpdate.Id); ;

                user.Name = userToUpdate.Name;
                user.Email = userToUpdate.Email;

                await userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating a user by id {UserId}.", userToUpdate.Id);
                throw;
            }
        }

        public async Task<ValidateUserResponseDto> LoginUserAsync(LoginDto loginDto)
        {
            var user = await userRepository
                .GetAsync(u => u.Username.Equals(loginDto.Username)
                            && u.Password.Equals(loginDto.Password));

            if (user is null)
            {
                return new ValidateUserResponseDto
                {
                    Success = false,
                    Errors = "Invalid user credentials."
                };
            }

            var userClaims = jwtTokenService.GenerateBasicClaimsForUser(mapper.Map<UserDto>(user));
            var (token, expirationMinutes) = jwtTokenService.GenerateAccessToken(userClaims);
            var dbRefreshtoken = await jwtTokenService.GetValidRefreshToken(user.Id);
            var newRefreshToken = jwtTokenService.GenerateRefreshToken();
            var refreshToken = new RefreshTokenDto
            {
                Token = newRefreshToken,
                UserId = user.Id
            };

            if (dbRefreshtoken is null)
            {
                refreshToken = await jwtTokenService.SaveRefreshToken(refreshToken);
            }
            else
            {
                refreshToken.ExpiryDate = dbRefreshtoken.ExpiryDate;
                refreshToken = await jwtTokenService.UpdateRefreshToken(refreshToken, setNewDate: false);
            }

            return new ValidateUserResponseDto
            {
                Success = true,
                AccessToken = token,
                AccessTokenExpirationMinutes = expirationMinutes,
                RefreshToken = newRefreshToken,
                RefreshTokenExpirationDate = refreshToken.ExpiryDate
            };
        }

        public async Task<ValidateUserResponseDto> RefreshUserTokenAsync(int userId, string oldRefreshToken)
        {
            var validRefreshToken = await jwtTokenService.ValidateRefreshToken(userId, oldRefreshToken);

            if (!validRefreshToken)
            {
                return new ValidateUserResponseDto
                {
                    Success = false,
                    Errors = "Invalid or expired refresh token."
                };
            }

            var user = await userRepository.GetAsync(u => u.Id == userId);
            var userClaims = jwtTokenService.GenerateBasicClaimsForUser(mapper.Map<UserDto>(user));
            var (token, expirationMinutes) = jwtTokenService.GenerateAccessToken(userClaims);
            var refreshToken = jwtTokenService.GenerateRefreshToken();

            var newRefreshToken = await jwtTokenService.UpdateRefreshToken(new RefreshTokenDto { Token = refreshToken, UserId = user.Id});

            return new ValidateUserResponseDto
            {
                Success = true,
                AccessToken = token,
                AccessTokenExpirationMinutes = expirationMinutes,
                RefreshToken = refreshToken,
                RefreshTokenExpirationDate = newRefreshToken.ExpiryDate
            };
        }

        public async Task RemoveRefreshToken(int userId)
        {
            await jwtTokenService.RemoveRefreshToken(userId);
        }

        private void ValidateUsername(string username)
        {
            if (username is null)
            {
                throw new InvalidUserDataException("Username is required.");
            }
            else if (username.Length == 0 && username.Length > 100)
            {
                throw new InvalidUserDataException("Username must be less than 100 characters.");
            }
        }

        private void ValidatePassword(string password)
        {
            if (password is null)
            {
                throw new InvalidUserDataException("Password is required.");
            }
            else if (password.Length < 8 && password.Length > 100)
            {
                throw new InvalidUserDataException("Password must be between 6 and 100 characters.");
            }
            else if (!RegexHelper.ValidPassword().IsMatch(password))
            {
                throw new InvalidUserDataException("Password must have minimum eight characters, at least one upper and one lower case letter, one number and one special character");
            }
        }
    }
}
