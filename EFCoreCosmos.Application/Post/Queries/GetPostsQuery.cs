using EFCoreCosmos.Application.Post.Models;
using EFCoreCosmos.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreCosmos.Application.Post.Queries
{
    public class GetPostsQuery : IGetPostsQuery
    {
        private readonly BlogDbContext _dbContext;

        public GetPostsQuery(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PostModel>> Execute()
        {
            return await _dbContext.Posts.Select(p => new PostModel
            {
                Author = p.Author.Name,
                Category = p.Category.Name,
                Id = p.Id,
                Title = p.Title
            }).ToListAsync();
        }
    }

    public interface IGetPostsQuery
    {
        Task<IEnumerable<PostModel>> Execute();
    }
}