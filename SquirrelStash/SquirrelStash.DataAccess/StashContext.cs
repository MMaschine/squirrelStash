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

            builder.Entity<Category>()
                .HasMany(e => e.Items)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Item>()
                .HasMany(e => e.PropertyEntries)
                .WithOne(e => e.Item)
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Category>()
                .HasMany(e => e.Properties)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PropertyDefinition>()
                .HasMany(e => e.Entries)
                .WithOne(e => e.Definition)
                .HasForeignKey(e => e.PropertyDefinitionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
