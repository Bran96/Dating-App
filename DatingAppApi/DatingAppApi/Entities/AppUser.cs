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
    }
}
