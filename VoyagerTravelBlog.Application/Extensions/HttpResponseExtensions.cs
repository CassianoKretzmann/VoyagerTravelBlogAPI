using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace VoyagerTravelBlog.Application.Extensions
{
    public static class HttpResponseExtensions
    {
        public static HttpResponse AddJwtCookie(this HttpResponse response, string jwtToken, int expirationMinutes)
        {
            response.Cookies.Append("jwt", jwtToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddMinutes(expirationMinutes)
            });

            return response;
        }

        public static HttpResponse AddRefreshTokenCookie(this HttpResponse response, string jwtToken, DateTimeOffset expirationDate)
        {
            response.Cookies.Append("refreshToken", jwtToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = expirationDate
            });

            return response;
        }

        public static HttpResponse CleanCookies(this HttpResponse response, HttpRequest request)
        {
            foreach (var cookiekey in request.Cookies.Keys)
            {
                response.Cookies.Delete(cookiekey);
            }

            return response;
        }
    }
}
