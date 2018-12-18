using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreCosmos.Domain
{
    public class Post
    {
        [Column("id")]
        public Guid Id { get; set; }
        public Author Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Category Category { get; set; }
    }
}