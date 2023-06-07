using System.Runtime.InteropServices;

namespace DatingAppApi.DTO_s
{
    public class MemberUpdateDto
    {
        // Inside here we specifying the properties that we allowing the user to update
        // Create a mapping from MemberUpdateDto to the AppUser in the AutoMapperProfiles.cs class
        public string Introduction { get; set; } = String.Empty;
        public string LookingFor { get; set; } = String.Empty;
        public string Interests { get; set; } = String.Empty;
        public string City { get; set; } = String.Empty;
        public string Country { get; set; } = String.Empty;
    }
}
