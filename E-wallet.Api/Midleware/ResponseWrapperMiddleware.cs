
using System.Text.Json;
namespace E_wallet.Api.Midleware
{
    public class ResponseWrapperMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseWrapperMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var originalBody = context.Response.Body;
            await using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            try
            {
                await _next(context);

                if (context.Response.StatusCode < 400) // Or a more specific check if needed
                {
                    // Rewind the memory stream to the beginning
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    var bodyAsText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                    context.Response.Body.Seek(0, SeekOrigin.Begin);

                    object? parsedBody = null;
                    if (!string.IsNullOrWhiteSpace(bodyAsText))
                    {
                        try { parsedBody = JsonSerializer.Deserialize<object>(bodyAsText); }
                        catch { parsedBody = bodyAsText; }
                    }

                    var wrappedResponse = new
                    {
                        success = true,
                        data = parsedBody,
                        errors = (object?)null
                    };

                    memoryStream.SetLength(0);
                    await context.Response.WriteAsync(JsonSerializer.Serialize(wrappedResponse));
                }
            }
            finally
            {
                memoryStream.Seek(0, SeekOrigin.Begin);

                await memoryStream.CopyToAsync(originalBody);

                context.Response.Body = originalBody;
            }
        }

    }
}
