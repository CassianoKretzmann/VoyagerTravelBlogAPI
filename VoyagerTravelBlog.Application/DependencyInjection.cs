using VoyagerTravelBlog.Application.Interfaces.Services;
using VoyagerTravelBlog.Application.Mappings;
using VoyagerTravelBlog.Application.Options;
using VoyagerTravelBlog.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VoyagerTravelBlog.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            services.Configure<JWTSettingsOptions>(configuration.GetSection(JWTSettingsOptions.JwtSettings));
            services.AddAutoMapper(typeof(MediaMapping), typeof(PostMapping), typeof(UserMapping), typeof(CommentMapping));
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<AuthenticatedUser>();

            if (!isDevelopment)
            {
                services.AddScoped<GlobalAntiforgeryFilter>();
            }

            return services;
        }
    }
}
