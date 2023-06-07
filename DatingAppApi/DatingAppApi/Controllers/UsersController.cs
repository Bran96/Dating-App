using AutoMapper;
using DatingAppApi.DTO_s;
using DatingAppApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DatingAppApi.Controllers
{
    [Authorize] // We want everyone to be authenticated before they can access all these endpoints inside this controller
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        //[AllowAnonymous] // This will allow users that aren't logged in will be able to see all the users
        [HttpGet]
        // We want to get a list of users thats why we use "IEnumerable"
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers() // We need to return the MemberDto inside the Angle brackets of ActionResult<>
        {
            var users = await _userRepository.GetMembersAsync();

            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username) // We need to return the MemberDto inside the Angle brackets of ActionResult<>
        {
            var user = await _userRepository.GetMemberAsync(username);

            if(user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // We gonna get the username from the token, because the user is updating their own profile and has a token
        // So we dont need the username in the route parameter for HttpPut, because we get that directly from the token
        // We  also dont need to return anything from this method, because the usr updating something knows what they updated
        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            // 1st thing we need to get hold of the username and that is inside the token
            // NameIdentifier because that is what we're using for our username
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // Now that we have our username 
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if(user == null)
            {
                return NotFound();
            }

            // Now in order to update the user, do the following
            // Now we can use our Mapper functionality to update the properties from our MemberUpdateDto into our user
            // This line of code is updating all the properties we pass through the memberUpdateDto and overrite the properties values what we have in the user
            _mapper.Map(memberUpdateDto, user); // 1st argument is mapping from the source object and 2nd argument is mapping to the source object

            // Now in order to save the changes to the database
            if(await _userRepository.SaveAllAsync())
            {
                // If successfull we gonna return NoContent() which means status code 204 - Is a great response for update but we dont have anything to send back, tahts the definition of 204
                return NoContent();
            }

            // If we havent made any changes to the database, we gonna return BadRequest
            return BadRequest("Failed to update user");
        }

    }
}
