using System.ComponentModel.DataAnnotations.Schema;

namespace DatingAppApi.Entities
{
    [Table("Photos")] // This is gonna be the new namme of the Table instead of the class Name that will be Photo only, we want it to be pluralized
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; } = String.Empty; // Url of where to find the photo
        public bool IsMain { get; set; } // Specify whether this is the user's main photo or not

        // When it comes to the photo upload, well use a technology to upload our photos and it will use a publicId
        public string PublicId { get; set; } = String.Empty;

        // One To Many relationship with AppUser
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}