using System.ComponentModel.DataAnnotations;

namespace DatingAppApi.DTO_s
{
    public class RegisterDto
    {
        // These are for validaton inside the brackets '[]'
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string KnownAs { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        public DateTime? DateOfBirth { get; set; } // Optional to make required work!

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;

    }
}
