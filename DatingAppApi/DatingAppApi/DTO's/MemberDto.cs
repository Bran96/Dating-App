namespace DatingAppApi.DTO_s
{
    public class MemberDto
    {
        // So this Member Dto is the properties that we want from the AppUser since we dont want all the properties to be returned from the AppUser Model
        public int Id { get; set; }
        public string UserName { get; set; } = String.Empty;

        // This PhotoUrl is gonna be null if we just put it in like this, because automapper does not know what to Map to since it isnt a known property in the AppUser Entity
        // So we need to tell AutoMapper how to get this property and populate this specific property or field and we do this in the AutoMapperProfiles class and in this case its for an individual property.
        // Use the .ForMember, look at the code.
        public string PhotoUrl { get; set; } // This is gonna be the user's main photo
        public int Age { get; set; }
        public string KnownAs { get; set; } = String.Empty;
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Gender { get; set; } = String.Empty;
        public string Introduction { get; set; } = String.Empty;
        public string LookingFor { get; set; } = String.Empty;
        public string Interests { get; set; } = String.Empty;
        public string City { get; set; } = String.Empty;
        public string Country { get; set; } = String.Empty;
        public List<PhotoDto> Photos { get; set; } // And each user can have many photos, but only one of them will be the able to be set as the main photo
    }
}
