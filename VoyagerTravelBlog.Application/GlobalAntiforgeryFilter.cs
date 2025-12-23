using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace VoyagerTravelBlog.Application
{
    public class GlobalAntiforgeryFilter(IAntiforgery antiforgery) : IAsyncActionFilter
    {
        private readonly IAntiforgery _antiforgery = antiforgery;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (
                context.HttpContext.Request.Method != "GET" &&
                context.HttpContext.Request.Method != "HEAD" &&
                context.HttpContext.Request.Method != "OPTIONS" &&
                context.HttpContext.Request.Method != "TRACE" &&
                context.HttpContext.Request.Path != "/api/auth/login" &&
                context.HttpContext.Request.Path != "/api/users"
            )
            {
                try
                {
                    await _antiforgery.ValidateRequestAsync(context.HttpContext);
                }
                catch (AntiforgeryValidationException ex)
                {
                    context.Result = new BadRequestObjectResult($"Invalid antiforgery token : {ex.Message}");
                    return;
                }
            }

            await next();
        }
    }
}
