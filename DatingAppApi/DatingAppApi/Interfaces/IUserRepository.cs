using DatingAppApi.DTO_s;
using DatingAppApi.Entities;

namespace DatingAppApi.Interfaces
{
    public interface IUserRepository
    {
        // These are the methods that we want our User Repository to be able to implement

        // Return void
        void Update(AppUser user);
        // Return boolean
        Task<bool> SaveAllAsync();
        //  Return am IEnumerable/List
        Task<IEnumerable<AppUser>> GetUsersAsync(); // We normally would use a list, but instead we rather use IEnumerable. Its more powerful, it lets us add or remove things from a list
        // Return a Single AppUser by id
        Task<AppUser> GetUserByIdAsync(int id);
        // Return a Single AppUser by username
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<IEnumerable<MemberDto>> GetMembersAsync();
        Task<MemberDto> GetMemberAsync(string username);
    }
}
