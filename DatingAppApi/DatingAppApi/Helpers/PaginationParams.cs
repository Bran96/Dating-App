namespace DatingAppApi.Helpers
{
    public class PaginationParams
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
    }
}
