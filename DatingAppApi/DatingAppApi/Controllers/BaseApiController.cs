using DatingAppApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppApi.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))] // This is for the Action filter we created to update the lastActive property inside the user
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
    }
}
