namespace DatingAppApi.Helpers
{
    public class CloudinarySettings
    {
        // These 3 properties or the configuration for CloudinarySettings spelling must be the same as in the appsettings.json file
        // And then we gonna add this helper class to our "ApplicationServiceExtensions" class

        public string CloudName { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }
}
