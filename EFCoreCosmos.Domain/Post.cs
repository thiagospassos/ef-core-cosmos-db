using System;

namespace EFCoreCosmos.Domain
{
    public class Post
    {
        public Guid Id { get; set; }
        public Author Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Category Category { get; set; }
    }
}