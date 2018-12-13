using System;

namespace EFCoreCosmos.Application.Post.Models
{
    public class PostModel
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
    }
}