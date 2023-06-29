using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            //We' ll mimic the try catch bloxk we want to add to every API
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                //log
                logger.LogError(ex, $"{errorId} : ex.Message");
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //Content type
                httpContext.Response.ContentType = "application/json";
                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something Went wrong"
                };

                await httpContext.Response.WriteAsJsonAsync(error); 

            }
        }
    }
}
