using EFCoreCosmos.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Cosmos.Metadata.Internal;
using System.Linq;
using System.Reflection;

namespace EFCoreCosmos.Persistence
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected BlogDbContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }


        //        protected override void OnModelCreating(ModelBuilder modelBuilder)
        //        {
        //            //If this is not set then the default name will be the name of the DbContext => BlogDbContext
        //            modelBuilder.GetInfrastructure().Cosmos(ConfigurationSource.Convention).DefaultContainerName = "Default";
        //
        ////            modelBuilder.Entity<Post>().Metadata.Cosmos().ContainerName = nameof(Posts);
        //            ////Manually setting container names for DbSets
        ////            modelBuilder.Entity<Category>().Metadata.Cosmos().ContainerName = nameof(Categories);
        //            
        ////            OneCollectionPerDbSet(modelBuilder);
        //
        //        }

        private void OneCollectionPerDbSet(ModelBuilder modelBuilder)
        {
            var dbSets = typeof(BlogDbContext).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType.IsGenericType && typeof(DbSet<>).IsAssignableFrom(p.PropertyType.GetGenericTypeDefinition()));
            foreach (var dbSet in dbSets)
            {
                var metadata = modelBuilder.Entity(dbSet.PropertyType.GetGenericArguments()[0]).Metadata;
                metadata.Cosmos().ContainerName = dbSet.Name;
            }
        }
    }
}
