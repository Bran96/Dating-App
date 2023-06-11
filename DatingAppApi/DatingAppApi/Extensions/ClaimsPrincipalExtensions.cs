using System.Security.Claims;

namespace DatingAppApi.Extensions
{
    // Creating an extension method to get the username since we gonna be dealing with that a lot in the http requests
    public static class ClaimsPrincipalExtensions
    {
        // We gonna return a string because that is our username
        // And the thing we gonna be extending is a claimsPrincipal inside the parentheses
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Getting the loggedIn user from our Token
        }
    }
}
