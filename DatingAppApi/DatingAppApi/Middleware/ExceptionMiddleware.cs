using DatingAppApi.Errors;
using System.Net;
using System.Text.Json;

namespace DatingAppApi.Middleware
{
    // This file is for any middleware we create in our application
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        // IHostEnvironment injection or argument allows us to see whether we run in development or production mode that is specified in the launchSettings.json file
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;            
        }

        // This method has to be called InvokeAsync because we telling our framework to recognize that this is middleware
        // If theres an exception anywhere in our application, this piece of middleware will catch it.
        public async Task InvokeAsync(HttpContext context) // HttpContext gives us access to the http request that is beeing passed through the middleware
        {
            // We gonna use a try catch block inside our middleware. This is gonna be responsible for handling the exceptions. We only need t do this one place and thats in our midleware
            try
            {
                await _next(context); // We just passing through the http context
            }
            catch (Exception ex) // Passing through the exception as an argument and call it ex
            {
                // This is the exact place where the exception will be handled
                // This is gonna output this information(error) into our terminal so that we can see whats going on whether we run in production or development mode
                // We gonna deceide what we gonna do with this exception in thhe following lines of code
                // START of the http response when we counter anexception
                _logger.LogError(ex, ex.Message);// Its gonna output the information in my terminal on whats going on whether we run in prod or dev
                context.Response.ContentType = "application/json"; // Here Im settig up the context and returning something to the client
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //  This is gonna give us an status code of 500

                //  Now we gonna check the environment to see whether we run in development mode or not
                // ternary operator to say what are we gonna do if we are in devlopment mode and what if we're not ? :
                var response = _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");

                // Now we gonna create and then return the response as JSON in the following 2 lines of code
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
                // START of the http response when we counter exception
                await context.Response.WriteAsync(json);
            }
        }
    }
}
