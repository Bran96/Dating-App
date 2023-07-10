using DatingAppApi.DTO_s;
using DatingAppApi.Entities;
using DatingAppApi.Extensions;
using DatingAppApi.Helpers;
using DatingAppApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingAppApi.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;
        public LikesRepository(DataContext context)
        {
            _context = context;
        }

        // This is going to find the UserLike entity that matches the primary key which is a combination of these 2 parameters passed in 
        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, targetUserId);
        }

        // Here we want to return a list of our UserLikes based on the predicate that we passing in here
        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams) // This userId could be the SourceUserId, which means we gonna get a list of users that are liked by the current loggedIn user. Or it could be the TargetUserId where we will get a list of users that user has liked
        {
            // 1st, Createing a query to get a list of users from the Database and order by their username
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            // 2nd query getting all the userLikes
            var likes = _context.Likes.AsQueryable();

            // 1st way around
            if(likesParams.Predicate == "liked") // To get the Users they themselves have liked, eg. The loggedIn user will see the users they have liked in the list
            {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.TargetUser); // This is gonna filter out or show the users based on the likes list
            }

            // 2nd way around, because we do many to many relationship
            if(likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(like => like.TargetUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }

            // Now we can go to our list of users and we can execute whats going to be contained inside that query
            var likedUsers = users.Select(user => new LikeDto
            {
                // We will be doing the mapping manually for this
                Id = user.Id,
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
                City = user.City,

            });

            return await PagedList<LikeDto>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
