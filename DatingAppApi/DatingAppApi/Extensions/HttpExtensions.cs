using DatingAppApi.Helpers;
using System.Text.Json;

namespace DatingAppApi.Extensions
{
    public static class HttpExtensions
    {
        // We're extending the HttpResponse and pass in the PaginationHeader class
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header)
        {
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            // Now we gonna access the response and the Headers and we gonna add an header called "Pagination"
            response.Headers.Add("Pagination", JsonSerializer.Serialize(header, jsonOptions));
            // Because this is a custom header we need to do something to allow cors policy inside here as well, otherwise the client will not be able to access the information inside this header
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination"); // This is what the client is expected to see when we expose the Header that we specified as "Pagination" which is our Header
        }
    }
}
