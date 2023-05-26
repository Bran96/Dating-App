using AutoMapper;
using DatingAppApi.DTO_s;
using DatingAppApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    }
}
