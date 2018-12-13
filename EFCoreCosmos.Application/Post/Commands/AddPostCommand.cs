using System;
using System.Threading.Tasks;
using EFCoreCosmos.Domain;
using EFCoreCosmos.Persistence;
using Serilog;

namespace EFCoreCosmos.Application
{
    public class AddPostCommand : IAddPostCommand
    {
        private readonly BlogDbContext _dbContext;

        public AddPostCommand(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Post> Execute(Post entity)
        {
            try
            {
                await _dbContext.Posts.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                Log
                    .ForContext("post", entity)
                    .Information("Post added successfully");
                return entity;
            }
            catch (Exception ex)
            {
                Log
                    .ForContext("post", entity)
                    .Error(ex, "Well, something bad happened");
            }

            return null;
        }
    }
    public interface IAddPostCommand
    {
        Task<Post> Execute(Post entity);
    }
}
