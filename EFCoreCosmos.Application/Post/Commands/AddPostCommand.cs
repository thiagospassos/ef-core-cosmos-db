using System;
using System.Threading.Tasks;
using EFCoreCosmos.Persistence;
using Serilog;

namespace EFCoreCosmos.Application.Post.Commands
{
    public class AddPostCommand : IAddPostCommand
    {
        private readonly BlogDbContext _dbContext;

        public AddPostCommand(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Domain.Post> Execute(Domain.Post entity)
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
        Task<Domain.Post> Execute(Domain.Post entity);
    }
}
