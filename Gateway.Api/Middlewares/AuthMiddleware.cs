namespace Gateway.Api.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            string authorizationHeaderValue = context.Request.Headers["Authorization"];

            // You can perform further validation or processing of the header value here

            await _next(context); // Continue processing the request
        }
        else
        {
            // If the header is not present, you can return a response with a 401 Unauthorized status
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
        }
    }
}