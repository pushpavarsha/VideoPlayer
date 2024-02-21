namespace VideoPlayer.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (System.Exception ex)
            {
                if (ex is BadHttpRequestException badRequestEx && badRequestEx.StatusCode == 413)
                {
                    context.Response.StatusCode = 413; 
                    await context.Response.WriteAsync("Custom error message: File size too large.");
                    return;
                }

                throw; 
            }
        }
    }
    public static class CustomExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}
