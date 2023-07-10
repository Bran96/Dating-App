namespace DatingAppApi.Helpers
{
    public class UserParams : PaginationParams // Now our userParams has all the properties inside our PaginationParams
    {
        public string CurrentUserName { get; set; } = String.Empty; // We dont want to display the current logged in user with the users list
        public string Gender { get; set; } = String.Empty; // We only want to display by default the opposite gender of the logged in user
        public int MinAge { get; set; } = 18; // Default value is 18 since its the youngest we allow to use this app
        public int MaxAge { get; set; } = 100; // Default age for Maximum age

        public string OrderBy { get; set; } = "lastActive"; // Default when the user is gonna sort the users, and that will be by 'lastActive'
    }
}
