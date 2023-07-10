using DatingAppApi.DTO_s;
using DatingAppApi.Entities;
using DatingAppApi.Extensions;
using DatingAppApi.Helpers;
using DatingAppApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppApi.Controllers
{
    public class LikesController : BaseApiController // we'll get to this controller by: api/likes
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;

        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _userRepository = userRepository;
            _likesRepository = likesRepository;
        }

        // When a user likes another user we need to use post
        [HttpPost("{username}")] // this route parameter "username" is the person they are about to like
        // Even though we are creating a resource on the server, all we gonna do is update our join table or add a new entry in the join table. And we are not going to return anything from this method
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId(); // This is gonna be the "user" liking another user
            var likedUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

            if(likedUser == null)
            {
                return NotFound();
            }

            if(sourceUser.UserName == username)
            {
                return BadRequest("You cannot like yourself");
            }

            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);

            if(userLike != null)
            {
                return BadRequest("You already like this user");
            }

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = likedUser.Id
            };

            // Now we gonna create the entity in the userLikes table
            sourceUser.LikedUsers.Add(userLike);

            // Now save it to the database
            if(await _userRepository.SaveAllAsync())
            {
                return Ok();
            }

            return BadRequest("Failed to like user");
        }

        // Now we gonna make use of the predicate to send back the likedUsers or the likedByUsers
        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams) // This is an object "LikeParams" instead of a query string, we need to add [FromQuery] since we only want to pass through the "predicate" string
        {
            likesParams.UserId = User.GetUserId();

            var users = await _likesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));

            return Ok(users);
        }

    }
}
