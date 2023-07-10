using DatingAppApi.Extensions;
using System.ComponentModel.DataAnnotations;

namespace DatingAppApi.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; } = String.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        // We getting the DOB, but Instead of showing the DateOfBirth we rather want to show the age of the user to the Client
        public DateTime DateOfBirth { get; set; } // DateOnly - allows us to only track the date of something
        public string KnownAs { get; set; } = String.Empty;
        public DateTime Created { get; set; } = DateTime.UtcNow; // Creating this property to keep track of when the user was created in our database and give an initial value of DateTime.UtcNow
        public DateTime LastActive { get; set; } = DateTime.UtcNow; // DateTime.UtcNow refers to GMT time
        public string Gender { get; set; } = String.Empty;
        public string Introduction { get; set; } = String.Empty;
        public string LookingFor { get; set; } = String.Empty;
        public string Interests { get; set; } = String.Empty;
        public string City { get; set; } = String.Empty;
        public string Country { get; set; } = String.Empty;
        public List<Photo> Photos { get; set; } = new List<Photo>(); // User can have many photos
        public List<UserLike> LikedByUsers { get; set; } // Who has liked the current user loggedIn. Many to Many relationship.
        public List<UserLike> LikedUsers { get; set; } // All the users the current User has Liked. Many to Many relationship
    }
}
