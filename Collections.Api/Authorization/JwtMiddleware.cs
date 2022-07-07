using Task4Back.Services;

namespace Task4Back.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            int? userId = jwtUtils.ValidateToken(token);

            if (userId != null)
            {
                Entities.User user = userService.GetById(userId.Value);

                if (user?.Status == true)
                {
                    context.Items["User"] = user;
                }
            }

            await _next(context);
        }
    }
}