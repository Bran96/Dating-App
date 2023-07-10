using DatingAppApi.DTO_s;
using DatingAppApi.Entities;
using DatingAppApi.Helpers;

namespace DatingAppApi.Interfaces
{
    public interface ILikesRepository
    {
        // We do not want to return our AppUsers from this
        // We much rather want to return an object that we can shape with just the information we getting from our Database when we do this. So create a new LikeDto to return custom Data
        Task<UserLike> GetUserLike(int sourceUserId, int targetUserId); // This is going to return the entity with just those 2 properties
        Task<AppUser> GetUserWithLikes(int userId); // This is gonna return our AppUser
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams); // Returning a list of LikeDto's. "Predicates" mean, do they want to get the users likes or the users they are liked by
    }
}
