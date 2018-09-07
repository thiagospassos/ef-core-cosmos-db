using System;

namespace EFCoreCosmoDbSample.Domain
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}