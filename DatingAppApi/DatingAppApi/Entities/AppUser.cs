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

        // Comment this out because the projectTo AutoMapping doesnt work well in the UserRepository when there is a method inside the entity that we want to map from
        // So add this as using .ForMember inside the AutoMapperProfiles class
        //public int GetAge() // Always Create an Extension Class for the Method in an entity
        //{
        //    return DateOfBirth.CalculateAge();
        //}
    }
}
