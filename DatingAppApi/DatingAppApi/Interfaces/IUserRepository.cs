using DatingAppApi.DTO_s;
using DatingAppApi.Entities;
using DatingAppApi.Helpers;

namespace DatingAppApi.Interfaces
{
    public interface IUserRepository
    {
        // These are the methods that we want our User Repository to be able to implement

        // Return void
        void Update(AppUser user);

        // Return boolean
        Task<bool> SaveAllAsync();

        // Return am IEnumerable/List
        // Not using
        Task<IEnumerable<AppUser>> GetUsersAsync(); // We normally would use a list, but instead we rather use IEnumerable. Its more powerful, it lets us add or remove things from a list

        // Return a Single AppUser by id
        // Not using
        Task<AppUser> GetUserByIdAsync(int id);

        // Return a Single AppUser by username
        // Not using
        Task<AppUser> GetUserByUsernameAsync(string username);

        // Using a MemberDto to get all Members instead of AppUser, because we dont want all the properties from the appUser to be shown to the user.
        // We can do this because we are making use of AutoMapper from the AppUser to the MemberDto
        // Using
        // This method is gonna be used for pagination, which means, we need to change the <IEnumerable> to "PagedList". We want a PagedList of Type MemberDto
        // And we also going to pass in the UserParams userParams
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);

        // Using a MemberDto to get a single Member by username instead of AppUser, because we dont want all the properties to be shown to the user
        // We can do this because we are making use of AutoMapper from the AppUser to the MemberDto
        // Using
        Task<MemberDto> GetMemberAsync(string username);
    }
}
