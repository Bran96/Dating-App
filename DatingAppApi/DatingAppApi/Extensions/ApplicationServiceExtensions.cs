using DatingAppApi.Data;
using DatingAppApi.Helpers;
using DatingAppApi.Interfaces;
using DatingAppApi.Services;
using Microsoft.EntityFrameworkCore;

namespace DatingAppApi.Extensions
{
    // Since this is an extension class for program.cs builder.services methods, we need to make this class static
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config) 
        {
            // ConnectionString
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("ConnectionString"));
            });

            // 1ST STEP ENABLING CORS FOR ANGULAR
            // Everything here can be injected in other classes in the project since we have it in this cvlass. It gives us that benefit.
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings")); // The name in the GetSection must match exactly the configurationKey that was added in the appSettings.json file
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();

            return services;
        }
    }
}
