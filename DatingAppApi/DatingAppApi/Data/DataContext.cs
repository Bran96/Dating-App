using DatingAppApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAppApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        // Always make the Tables in the Database to be pluralized
        // Adding DbSets to add tables to the database
        // The table names will be the property names of the DbSet
        // The column names will be the property names inside the model between the DbSet tags <>
        public DbSet<AppUser> Users { get; set; }

        // We could add another DbSet for Photos,
        // but when a user adds a specific photo, its gonna be added to that specific user and the user wont be able to add photos for any other user
        // We also dont need to query the photos directly.
        // We will not go to the Database and ask for a specific photo for a random user
        // So based on the 4th point we dont need a DbSet for Photos, because we gonna query the photo via the User Entity which is the (AppUser)Users Table

    }
}
