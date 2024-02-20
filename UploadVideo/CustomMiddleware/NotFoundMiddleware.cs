using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;

namespace VideoPlayer.CustomMiddleware
{
    public class NotFoundMiddleware
    {
        readonly RequestDelegate _next;

        public NotFoundMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next.Invoke(context);

            if (context.Response.StatusCode == 404)
            {
                var errorMessage =
                    JsonConvert.SerializeObject(
                        new UploadVideoAPI.Exception.NotFoundException("Url is invalid", "Url").ToErrorResult());
                await context.Response.WriteAsync(errorMessage);
            }
        }
    }
}
