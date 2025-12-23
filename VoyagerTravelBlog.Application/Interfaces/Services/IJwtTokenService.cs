using VoyagerTravelBlog.Application.Dtos.Auth;
using VoyagerTravelBlog.Application.Dtos.User;
using System.Security.Claims;

namespace VoyagerTravelBlog.Application.Interfaces.Services
{
    public interface IJwtTokenService
    {
        IEnumerable<Claim> GenerateBasicClaimsForUser(UserDto user);
        (string token, int expirationMinutes) GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        Task<RefreshTokenDto> SaveRefreshToken(RefreshTokenDto refreshToken);
        Task<RefreshTokenDto> UpdateRefreshToken(RefreshTokenDto refreshToken, bool setNewDate = true);
        Task RemoveRefreshToken(int userId);
        Task<bool> ValidateRefreshToken(int userId, string refreshToken);
        Task<RefreshTokenDto> GetValidRefreshToken(int userId);
    }
}
