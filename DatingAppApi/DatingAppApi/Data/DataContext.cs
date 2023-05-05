using DatingAppApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAppApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        // Adding DbSets to add tables to the database
        // The table names will be the property names of the DbSet
        // The column names will be the property names inside the model between the DbSet tags <>
        public DbSet<AppUser> Users { get; set; }
    }
}
