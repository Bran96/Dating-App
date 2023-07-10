namespace DatingAppApi.Entities
{
    // This is gonna Act as a Join Table between AppUser and AppUser itself
    public class UserLike
    {
        public AppUser SourceUser { get; set; } // This is gonna be the loggedIn user that will have a list of users that he/she liked
        public int SourceUserId { get; set; }

        public AppUser TargetUser { get; set; } // The user that is getting liked by the SourceUser
        public int TargetUserId { get; set; }


    }
}
