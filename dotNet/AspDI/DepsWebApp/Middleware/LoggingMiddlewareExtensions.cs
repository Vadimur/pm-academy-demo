using Microsoft.AspNetCore.Builder;

namespace DepsWebApp.Middleware
{
#pragma warning disable CS1591
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
#pragma warning restore CS1591
}
