using AutoMapper;
using VoyagerTravelBlog.Application.Dtos.Auth;
using VoyagerTravelBlog.Application.Dtos.User;
using VoyagerTravelBlog.Application.Exceptions;
using VoyagerTravelBlog.Application.Interfaces.Repositories;
using VoyagerTravelBlog.Application.Interfaces.Services;
using VoyagerTravelBlog.Application.Options;
using VoyagerTravelBlog.Domain.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace VoyagerTravelBlog.Application.Services
{
    public class JwtTokenService(
        IConfiguration configuration,
        ILogger<JwtTokenService> logger,
        IOptions<JWTSettingsOptions> jwtSettings,
        IMapper mapper, 
        IRefreshTokenRepository refreshTokenRepository) : IJwtTokenService
    {
        private readonly ILogger<JwtTokenService> logger = logger;
        private readonly IMapper mapper = mapper;
        private readonly IRefreshTokenRepository refreshTokenRepository = refreshTokenRepository;
        private readonly JWTSettingsOptions jwtSettings = jwtSettings.Value;

        public (string token, int expirationMinutes) GenerateAccessToken(IEnumerable<Claim> claims)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: jwtSettings.Issuer,
                    audience: jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(jwtSettings.AccessTokenExpirationMinutes),
                    signingCredentials: creds
                );

                return (token: new JwtSecurityTokenHandler().WriteToken(token), expirationMinutes: jwtSettings.AccessTokenExpirationMinutes);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while generating an access token.");
                throw;
            }
        }

        public IEnumerable<Claim> GenerateBasicClaimsForUser(UserDto user)
        {
            return
            [
                // Custom claim using the user's ID.
                new("Myapp_User_Id", user.Id.ToString()),
                // Standard claim for user identifier, using username.
                new(ClaimTypes.NameIdentifier, user.Username),
                // Standard claim for user's email.
                new(ClaimTypes.Email, user.Email),
                // Standard JWT claim for subject, using user ID.
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            ];
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber); ;
        }

        public async Task<RefreshTokenDto> SaveRefreshToken(RefreshTokenDto refreshToken)
        {
            try
            {
                var hashedToken = HashToken(refreshToken.Token);
                refreshToken.Token = hashedToken;
                refreshToken.ExpiryDate = DateTimeOffset.Now.AddDays(jwtSettings.RefreshTokenExpirationDays);

                var createdRefreshToken = await refreshTokenRepository.CreateAsync(mapper.Map<RefreshToken>(refreshToken));

                return mapper.Map<RefreshTokenDto>(createdRefreshToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while saving a refresh token.");
                throw;
            }
        }

        public async Task<RefreshTokenDto> UpdateRefreshToken(RefreshTokenDto refreshToken, bool setNewDate = true)
        {
            try
            {
                var hashedToken = HashToken(refreshToken.Token);
                var refreshTokenToUpdate = await refreshTokenRepository.GetAsync(r => r.UserId == refreshToken.UserId) ?? throw new NotFoundException(nameof(RefreshToken), nameof(User), refreshToken.UserId);

                refreshTokenToUpdate.Token = hashedToken;
                refreshTokenToUpdate.ExpiryDate = setNewDate ? DateTimeOffset.Now.AddDays(jwtSettings.RefreshTokenExpirationDays) : refreshToken.ExpiryDate;

                await refreshTokenRepository.UpdateAsync(refreshTokenToUpdate);

                return mapper.Map<RefreshTokenDto>(refreshTokenToUpdate);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating a refresh token.");
                throw;
            }
        }

        public async Task RemoveRefreshToken(int userId)
        {
            try
            {
                var refreshTokenToRemove = await refreshTokenRepository.GetAsync(r => r.UserId == userId) ?? throw new NotFoundException(nameof(RefreshToken), nameof(User), userId);

                await refreshTokenRepository.RemoveAsync(refreshTokenToRemove);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while removing a refresh token.");
                throw;
            }
        }

        public async Task<bool> ValidateRefreshToken(int userId, string refreshToken)
        {
            var storedToken = await refreshTokenRepository.GetAsync(r => r.UserId == userId);

            if (storedToken is null || storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return false;
            }

            return VerifyToken(storedToken.Token, refreshToken);
        }

        public async Task<RefreshTokenDto> GetValidRefreshToken(int userId)
        {
            try
            {
                var storedToken = await refreshTokenRepository.GetAsync(r => r.UserId == userId && r.ExpiryDate > DateTimeOffset.Now);

                return this.mapper.Map<RefreshTokenDto>(storedToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting a valid refresh token.");
                throw;
            }
        }

        private string HashToken(string token)
        {
            var salt = new byte[128 / 8];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: token,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        private bool VerifyToken(string storedHash, string providedToken)
        {
            var parts = storedHash.Split('.');
            if (parts.Length != 2)
                return false;

            var salt = Convert.FromBase64String(parts[0]);
            var hash = parts[1];

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: providedToken,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hash == hashed;
        }
    }
}
