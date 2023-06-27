using AutoMapper;
using DatingAppApi.DTO_s;
using DatingAppApi.Entities;
using DatingAppApi.Extensions;
using DatingAppApi.Helpers;
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
        private readonly IPhotoService _photoService;
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        //[AllowAnonymous] // This will allow users that aren't logged in will be able to see all the users
        [HttpGet]
        // We want to get a list of users thats why we use "IEnumerable"
        // For the UserParams we passing tthrough, we gona ask the client to send this up as a query string and not just an object, by doing this we will be adding [FromQuery]
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery]UserParams userParams) // We need to return the MemberDto inside the Angle brackets of ActionResult<>
        {
            // This is for getting the current user thats logged in that we dont want to display in the members list when we click on "Matches"
            var username = User.GetUsername();
            var currentUser =  await _userRepository.GetUserByUsernameAsync(username);
            userParams.CurrentUserName = currentUser.UserName;

            // We want our users to select which gender they want to view, but if they do not make a selection or they just loaded the members page which is the matches tab
            // then we going to send back a default instead
            if(string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = currentUser.Gender == "male" ? "female" : "male";
            }

            var users = await _userRepository.GetMembersAsync(userParams);

            // We also want to return our Pagination information via the Pagination Header, so we gonna get access to our httpResponse which is called "Response" inside our ApiContoller
            // and then add our "AddPaginationHeader" in the HttpExtensions Class
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));

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
            // NameIdentifier because that is what we're using for our username, but it is now in the ClaimsPrincipalExtension
            var username = User.GetUsername();// Getting the loggedIn User
            // Now that we have our username, now we get the etire user's details
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

        // Uploading a photo
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file) // When testing this method, the argument "file" must match the key value we have in postman if we click on body and then form-data
        {
            var username = User.GetUsername(); // Getting the username from the token that is in the ClaimsPrincipalExtensions
            // This user is an object
            var user = await _userRepository.GetUserByUsernameAsync(username); // Now we getting the user's details according to te username we got

            if(user == null)
            {
                return NotFound();
            }

            // Uploading the image
            var result = await _photoService.AddPhotoAsync(file);

            // Check the result to see if we get an error when uploading a photo. We do have an error object to check when uploading a photo
            if(result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            // If we do have the result(the photo), we can go ahead and create our photo in the DB
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,

            };

            // We also want to see if this is the user's first photo their uploading and if it is, then we gonna set it to main
            if(user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo); // Entity Framework is tracking this in memory

            if(await _userRepository.SaveAllAsync() ) // If there are changes saved into our database, this is what the if statement state there.
            {
                // We gonn Map into our photoDto from our photo
                //return _mapper.Map<PhotoDto>(photo); // Instead of returning this we gonna return something else: And this is gonna return a 201 created response with location details of where to find the newly created resource and also send back the newly created resource
                return CreatedAtAction(nameof(GetUser), new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));
            }

            // If we do not save successfully we gonna return a BadRequest()
            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var username = User.GetUsername(); // Getting the username from the token that is in the ClaimsPrincipalExtensions
            var user = await _userRepository.GetUserByUsernameAsync(username); // Now we getting the user's details according to te username we got

            if(user == null )
            {
                return NotFound();
            }

            // Now we want to get hold of the photo from the User
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId); //FirstOrDefault can return null

            if(photo == null)
            {
                return NotFound();
            }

            // We want to check if this photo is already the user's main photo. If it is we dont want to allow the user to set the photo as main again
            if(photo.IsMain)
            {
                return BadRequest("This is already your main photo");
            }

            // Then we gonna see what the current Main photo is of the user
            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            // Check if the currentMain photo is not null, in other words isMain is set to true
            if(currentMain != null)
            {
                currentMain.IsMain = false;
            }

            // Now we're setting the new photo that we're updating's property "isMain" to true
            photo.IsMain = true;

            // Now save the changes that are tracked by entity framework to our database
            if(await _userRepository.SaveAllAsync())
            {
                return NoContent(); // We're using NoContent, because its justt an update
            }

            // If the save fails we gonna return a badRequest
            return BadRequest("Problem setting main photo");
            // No go to postman and check the result
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var username = User.GetUsername(); // Getting the username from the token that is in the ClaimsPrincipalExtensions
            var user = await _userRepository.GetUserByUsernameAsync(username); // Now we getting the user's details according to te username we got

            // Check if we have a loggedIn user
            if(user == null)
            {
                return NotFound();
            }

            // Now we want to get hold of the photo from the User
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if(photo == null)
            {
                return NotFound();
            }

            // We gonna check if this photo is the users main photo, because we cant delete it then
            if(photo.IsMain)
            {
                return BadRequest("You cannot delete your main photo");
            }

            if(photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null)
                {
                    return BadRequest(result.Error.Message);
                }
            }

            user.Photos.Remove(photo); // EF is tracking that we're removing a record in the database

            if(await _userRepository.SaveAllAsync())
            {
                return Ok();
            }

            // If we did not save changes successfully then we gonna return a bad request:
            return BadRequest("Problem deleting photo");

        }
    }
}
