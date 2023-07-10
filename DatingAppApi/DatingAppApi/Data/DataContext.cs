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

        public DbSet<UserLike> Likes { get; set; }
        // For this to work we need to to override a method provided in the DbContext that we're extending in this class. Classes that has "Virtual" in can be overrided.
        // The method that we need to override is called OnModelCreating(ModelBuilder modelBuilder) from DbContext. And look at the summary text
        // Instead of using Migrations this is how we would create a many to many link without using the help of EF to create a migration
        // Once this is done then we create a new Migration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // We gonna use our builder and target the entity that we are interested in which is the UserLike
            // Then we gonna specify in here how we want to configure our relationship
            modelBuilder.Entity<UserLike>().HasKey(k => new { k.SourceUserId, k.TargetUserId });
            // Configure the 1st side of the relationship now.
            modelBuilder.Entity<UserLike>()
                .HasOne(s => s.SourceUser) // For Instance the SourceUser is Lisa which is the loggedIn User
                .WithMany(l => l.LikedUsers) // Peter would be one of the LikedUsers. In the end it means Lisa can like Peter, Paul, Tom etc.. So that is what we mean with many here. SourceUser(LoggedIn User) can like many other Users
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade); // If we were to allow our users to delete their profile from our application, then we would also want to delete the corresponding likes in the Database as well, thats why we need this OnDelete
            // Configure the other side(2nd side) of the relationship now
            modelBuilder.Entity<UserLike>()
                .HasOne(s => s.TargetUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.TargetUserId)
                .OnDelete(DeleteBehavior.NoAction); // Since, were using Sql Server, we need to specify one of the Many to Many's Delete Behaviour as "NoAction", otherwise we'll get an error. If we were to allow our users to delete their profile from our application, then we would also want to delete the corresponding likes in the Database as well, thats why we need this OnDelete

        }
    }
}
