
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

                memoryStream.Seek(0, SeekOrigin.Begin);
                string rawBody = await new StreamReader(memoryStream).ReadToEndAsync();

                object? parsedBody = null;
                if (!string.IsNullOrWhiteSpace(rawBody))
                {
                    try
                    {
                        parsedBody = JsonSerializer.Deserialize<object>(rawBody);
                    }
                    catch
                    {
                        parsedBody = rawBody; // في حال لم يكن JSON صالح
                    }
                }

                bool isSuccess = context.Response.StatusCode >= 200 && context.Response.StatusCode < 300;

                var response = new
                {
                    success = isSuccess,
                    data = isSuccess ? parsedBody : null,
                    errors = isSuccess ? null : parsedBody
                };

                context.Response.Body = originalBody;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    success = false,
                    data = (object?)null,
                    errors = ex.Message
                };

                context.Response.Body = originalBody;
                context.Response.StatusCode = 400; // يمكن تغييره حسب نوع الخطأ
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
        }

    }
}
