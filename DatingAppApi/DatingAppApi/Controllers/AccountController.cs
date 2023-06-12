using AutoMapper;
using DatingAppApi.Data;
using DatingAppApi.DTO_s;
using DatingAppApi.Entities;
using DatingAppApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingAppApi.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(DataContext dataContext, ITokenService tokenService, IMapper mapper)
        {
            _context = dataContext;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        // We are using a Post because we registering a user on the system / Adding a user on the system
        [HttpPost("register")] // POST: api/account/register
        // Difference between the AppUser and the registerDto:
        // - We are returning the AppUser in the ActionResult to confirm that what we've done has been created
        // - We are receiving the registerDto whhen the user register which is the username and the password only
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            // Check if the usernname already exists
            if (await UserExists(registerDto.Username))
            {
                return BadRequest("Username is taken");
            }

            // Create the autoMapper to the AppUser from the registerDto. In other words we creating the user here with automapper since their properties are the same.
            var user = _mapper.Map<AppUser>(registerDto);

            // We need to hash the password that the user is entering when registering, by using the hashing algorithm,.Net Framework provide us with hashing algorithm
            // The algorithm im using is called HMAC
            // Once we are finished with this class "hmac" we want to dispose it. And if we want to let that happen automatically, we use using "using" in front
            using var hmac = new HMACSHA512();

            // So when we are finished with this class which is the using then the dispose method is gonna get called
            // And now we gonna create a new user and specify the properties
            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;

            // This tracks the new entity in our memory, doesn't do anything to the database, but we're telling EF that we want to add our user
            await _context.Users.AddAsync(user);
            // Now we're saving it to the database
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            // First we need to get the AppUser from the database because we want to login and check if the user can login with the credentials thats already stored in the database.
            // We can either use first or SingledefaultAsync
            var user = await _context.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if(user == null)
            {
                return Unauthorized("Invalid username");
            }

            // Now we want to make sure that the password is also correct
            // This returns a byte array
            using var hmac = new HMACSHA512(user.PasswordSalt);
            
            // This returns a byte array
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            //Now we need to compare the 2 byte arrays 
            for(int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid Password");
                }
            }

            // If they make it pass the for loop then we know that the passwords match
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url, // We just add optional chaining here incase the user doesnt have a main photo yet
                KnownAs = user.KnownAs,
            };
        }


        // Creating a private method
        // Prevent the username of register in the "register controller method" to be the same username to be registered thats already in the Users table
        // We gonna return a boolean from this which is in the Task
        private async Task<bool> UserExists(string username)
        {
            // The first 'x' refers to the AppUser Table which is the Users table and the AnyAsync will loop over every user to see if the username already exists in this table
            //  If username does exist it will return true. If the username does not exist it wiill return false
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
