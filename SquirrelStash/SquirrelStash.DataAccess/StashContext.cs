using Microsoft.EntityFrameworkCore;
using SquirrelStash.DataAccess.Entities;


namespace SquirrelStash.DataAccess
{
    public class StashContext(DbContextOptions<StashContext> options) : DbContext(options)
    {
        public DbSet<Item> Items { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<PropertyDefinition> PropertyTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Item>().HasKey(e => e.Id);
            builder.Entity<Category>().HasKey(e => e.Id);
            builder.Entity<PropertyDefinition>().HasKey(e => e.Id);
            builder.Entity<PropertyEntry>().HasKey(e => e.Id); 

            builder.Entity<Item>()
                .HasOne(e => e.Category)
                .WithMany(e => e.Items)
                .HasForeignKey(e => e.CategoryId);

            builder.Entity<PropertyDefinition>()
                .HasOne(e => e.Category)
                .WithMany(e => e.Properties)
                .HasForeignKey(e => e.CategoryId);

            builder.Entity<PropertyEntry>()
                .HasOne(e => e.Item)
                .WithMany(e => e.PropertyEntries)
                .HasForeignKey(e => e.ItemId);
        }
    }
}
