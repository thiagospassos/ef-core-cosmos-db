using EFCoreCosmos.Application.Post.Models;
using EFCoreCosmos.Persistence;
using Serilog;
using System;
using System.Threading.Tasks;

namespace EFCoreCosmos.Application.Post.Commands
{
    public class AddPostCommand : IAddPostCommand
    {
        private readonly BlogDbContext _dbContext;

        public AddPostCommand(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PostModel> Execute(PostModel model)
        {
            var entity = new Domain.Post();
            try
            {
                model.Bind(entity);
                await _dbContext.Posts.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                Log
                    .ForContext("post", entity)
                    .Information("Post added successfully");
                return PostModel.Projection.Compile().Invoke(entity);
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
        Task<PostModel> Execute(PostModel model);
    }
}
