using System.Configuration;

namespace MRSAPI
{
    public class CustomHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ExpectedApiKey = "THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING";
        //private const string CustomHeader = "X-Custom-Header";
        //private const string ApiKeyHeader = "My custom value";

        private readonly IConfiguration _configuration;

        public CustomHeaderMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var appSettingsSection = _configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            var clientId = appSettings.ClientId;
            var clientSecret = appSettings.ClientSecret;
            var reqClientId = context.Request.Headers["ClientId"];
            var reqClientSecret = context.Request.Headers["ClientSecret"];

            if (clientSecret != reqClientSecret || clientId != reqClientId)  
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Invalid API key");
                return;
            }
            await _next(context);
        }
    }
}
