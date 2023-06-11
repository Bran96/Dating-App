using CloudinaryDotNet.Actions;

namespace DatingAppApi.Interfaces
{
    public interface IPhotoService
    {
        //Both these 2 methods are gonna return Task and their async methods

        // The ImageUploadResult that we getting back from Cloudinary will give us the "PublicId" which we gonna store in our Database, so when we need to delete a photo we have then access to the publicId in the 2nd method here.
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file); // We gonna return ImageUploadResult which is inside the anc tags of Task<>
        Task<DeletionResult> DeletePhotoAsync(string publicId); // We gonna return DeletionResult which is inside the anc tags of Task<>
    }
}
