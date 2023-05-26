namespace DatingAppApi.Extensions
{
    public static class DateTimeExtensions
    {
        // Because this is an extension method we also need to specify what we are about to be extending.
        // And in this case its the Date only type that we need to pass through in the parentheses that we gonna be extending.
        public static int CalculateAge(this DateTime dob)
        {
            // Get the value of what today is:
            var today = DateTime.Today;

            var age = today.Year - dob.Year;

            if (dob.Date > today.AddYears(-age)) age--;

            // This is not a super accurate calculation because we have not accounted for leap years, so there may be a chance that this is not 100% accurate
            return age;
        }
    }
}
