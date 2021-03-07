using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IO;

namespace DepsWebApp.Middleware
{
#pragma warning disable CS1591
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string requestBody = await ObtainRequestBody(context.Request);
            string request = "Logged Request Information: " +
                             $"Schema:{context.Request.Scheme} " +
                             $"Host: {context.Request.Host} " +
                             $"Path: {context.Request.Path} " +
                             $"QueryString: {context.Request.QueryString} " +
                             $"Request Body: {requestBody}";
            
            _logger.LogInformation($"[CustomLoggingMiddleware] {request}");
            
            var originalResponseBodyStream = context.Response.Body;
            
            await using var newResponseBodyStream = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = newResponseBodyStream;
            
            await _next(context);

            string responseBody = await ObtainResponseBody(context);
            string response = "Logged Response Information: " +
                             $"Schema:{context.Request.Scheme} " +
                             $"Host: {context.Request.Host} " +
                             $"Path: {context.Request.Path} " +
                             $"QueryString: {context.Request.QueryString} " +
                             $"Response Body: {responseBody}";
            _logger.LogInformation($"[CustomLoggingMiddleware] {response}");
            
            await newResponseBodyStream.CopyToAsync(originalResponseBodyStream);
        }
        
        private async Task<string> ObtainRequestBody(HttpRequest request)
        {
            if (request.Body == null)
            {
                return string.Empty;
            }
            
            request.EnableBuffering();
            
            var encoding = GetEncodingFromContentType(request.ContentType);
            
            string bodyStr;
            using (var reader = new StreamReader(request.Body, encoding, true, 1024, true))
            {
                bodyStr = await reader.ReadToEndAsync().ConfigureAwait(false);
            }
            request.Body.Seek(0, SeekOrigin.Begin);
            
            return bodyStr;
        }

        private async Task<string> ObtainResponseBody(HttpContext context)
        {
            var response = context.Response;
            response.Body.Seek(0, SeekOrigin.Begin);
            
            var encoding = GetEncodingFromContentType(response.ContentType);
            using var reader = new StreamReader(response.Body, encoding, detectEncodingFromByteOrderMarks: false,
                
                bufferSize: 4096, leaveOpen: true);
            var text = await reader.ReadToEndAsync().ConfigureAwait(false);
            response.Body.Seek(0, SeekOrigin.Begin);
            
            return text;
        }
        
        private Encoding GetEncodingFromContentType(string contentTypeStr)
        {
            if (string.IsNullOrEmpty(contentTypeStr))
            {
                return Encoding.UTF8;
            }
            ContentType contentType;
            try
            {
                contentType = new ContentType(contentTypeStr);
            }
            catch (FormatException)
            {
                return Encoding.UTF8;
            }
            if (string.IsNullOrEmpty(contentType.CharSet))
            {
                return Encoding.UTF8;
            }
            return Encoding.GetEncoding(contentType.CharSet, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);
        }
        
    }
#pragma warning restore CS1591
}
