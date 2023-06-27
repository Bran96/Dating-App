namespace DatingAppApi.Helpers
{
    public class UserParams
    {
        // Inside here, we're only going to alllow the user to choose how many items per page they want and not request all the users which might be a million users.
        // So we gonna allow the client to choose how many items per page they want,but we are going to set a maximum page size that they can request

        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1; // We're always returning the 1st page unless they tell us otherwise
        // We're using underscore for private properties
        private int _pageSize = 10; // Setting the initial value to 10 unless they tell us otherwise. This is how many we gonna return unless they want a different page size
        public int PageSize
        {
            // Now we gona get and set the _pageSize
            get => _pageSize; // This is gonna get the value of 10
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value; // Checks the value against the MaxPageSize

        }

        public string CurrentUserName { get; set; } = String.Empty; // We dont want to display the current logged in user with the users list
        public string Gender { get; set; } = String.Empty; // We only want to display by default the opposite gender of the logged in user
        public int MinAge { get; set; } = 18; // Default value is 18 since its the youngest we allow to use this app
        public int MaxAge { get; set; } = 100; // Default age for Maximum age

        public string OrderBy { get; set; } = "lastActive"; // Default when the user is gonna sort the users, and that will be by 'lastActive'
    }
}
