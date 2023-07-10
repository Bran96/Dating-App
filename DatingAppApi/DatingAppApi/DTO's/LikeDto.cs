namespace DatingAppApi.DTO_s
{
    // This is just gonna contain the properties that we want to display inside a member card when a user displays the list of users that they have liked
    public class LikeDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public string PhotoUrl { get; set; }
        public string City { get; set; }
    }
}
