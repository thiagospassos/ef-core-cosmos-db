using EFCoreCosmoDbSample.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCoreCosmoDbSample.Persistence
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions options) : base(options)
        {
        }

        protected BlogDbContext()
        {
        }
        public DbSet<Post> Posts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>();
            var blog = modelBuilder.Entity<Post>().Metadata;
            blog.CosmosSql().CollectionName = nameof(Posts);
        }
    }
}