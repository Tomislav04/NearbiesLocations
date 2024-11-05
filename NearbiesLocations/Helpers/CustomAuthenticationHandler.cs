namespace NearbiesLocations.Helpers
{
    public class CustomAuthenticationHandler
    {
        private readonly RequestDelegate _next;

        public CustomAuthenticationHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                {
                    Message = "Neispravan Username ili API ključ."
                }));
            }

            if (context.Response.StatusCode == StatusCodes.Status500InternalServerError)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                {
                    Message = "Greška sustava."
                }));
            }
        }
    }
}
