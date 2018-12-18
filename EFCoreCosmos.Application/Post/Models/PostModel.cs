using System;
using System.Linq.Expressions;
using System.Linq;

namespace EFCoreCosmos.Application.Post.Models
{
    public class PostModel
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public static Expression<Func<Domain.Post, PostModel>> Projection => (p) => new PostModel
        {
            Author = p.Author.Name,
            Category = p.Category.Name,
            Id = p.Id,
            Title = p.Title
        };

        public void Bind(Domain.Post entity)
        {
            entity.Title = this.Title;
            entity.Author = new Domain.Author() { Name = this.Author };
            entity.Category = new Domain.Category() { Name = this.Category };
            entity.Content = this.Content;
        }
    }

}