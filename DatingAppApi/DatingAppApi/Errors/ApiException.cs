namespace DatingAppApi.Errors
{
    // This class is gonna contain the response what we gonna send back to the client when we have an exception
    public class ApiException
    {
        // These are the arguments when we create a new ApiException then we can pass through the statusCode, message and the details here.
        // So this is what we gonna use to return our exception
        public ApiException(int statusCode, string message, string details)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; } // This property is gonna take the stack trace
    }
}
