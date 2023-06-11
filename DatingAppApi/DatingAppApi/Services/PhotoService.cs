using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingAppApi.Helpers;
using DatingAppApi.Interfaces;
using Microsoft.Extensions.Options;

namespace DatingAppApi.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        // We need to create a constructor because we neeed to inject our configuration into this
        // Because we created a strongly typed class for our configuration which is the CloudinarySettings.cs class, this is how we access that in the parentheeses of the constructor:
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            // Next we gonna see if we have something in the "file" object that we pass through in the parentheses of this method
            if(file.Length > 0) // Now we know we have a file to work with
            {
                // Access a stream of that file so we can upload that stream of stuff or the file to cloudinary
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = "dating-app"
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;


        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            // The destroyAsync will actually return us the DeletionResult and that is what we are returning from this method
            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}
