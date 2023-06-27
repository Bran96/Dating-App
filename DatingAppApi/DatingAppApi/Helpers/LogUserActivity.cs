using DatingAppApi.Extensions;
using DatingAppApi.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatingAppApi.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        // context is doing something before the api has executed
        // next is doing something after the api has been executed
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // This means that the api action has executed and we gonna get a result context back from this which will be stored in this variable
            // So we gonna choose next and wait for the api to be done with its job, and then we gonna do something in order to update the lastActive property inside the user
            var resultContext = await next();

            // So we want this to be utilized if the user is authenticated, because we want to update the property insidethe user, so ofcourse we want to see if the user is authenticated
            // Getting access to the HttpContext for this request and then go to the user property in the claimsPrincipal and then specify identity and check the IsAuthenticated property inside there
            // This code is also just for authenticated users
            if(!resultContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // If the user is not authenticated we just gonna return
                return;
            }

            // We want to get the id for this user from the claimsPrincipleExtension
            var userId = resultContext.HttpContext.User.GetUserId();

            // Now we also need to get access to our UserRepository because we going to update something for our user
            var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

            // Get the entire user object by making use of this repo
            var user = await repo.GetUserByIdAsync(userId);
            // TThis is our Action filter, it has one job to update one property from the user we're getting from our repository
            //  And our action filters give us access to the HttpContext and do things like get hold oof our users username and our services so we can get access to the repos and interfaces
            user.LastActive = DateTime.UtcNow; // So once this api gets executed since we're using "next", the lastActive property is gonna update immediatly to todays Date
            repo.SaveAllAsync(); // We then update our database in this line

            // At the end of this file we gonna add this as a service like any other service and add it to our ApplicationServiceExtensions class
            // We must also be very specific where we gonna use this "ActionFilter" we can either specify it in the UsersController or BaseApiController using the [ServiceFilter(typeof(LogUserActivity))]
        }
    }
}