using Collections.Api.Services;

namespace Collections.Api.Authorization;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateToken(token);
        if (userId != null)
        {
            var user = await userService.GetById(userId.Value);
            if (user?.Status == true)
            {
                context.Items["User"] = user;
            }
        }
        await _next(context);
    }
}
