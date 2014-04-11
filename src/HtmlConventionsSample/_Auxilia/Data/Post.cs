using System;

namespace HtmlConventionsSample._Auxilia.Data
{
    public class Post
    {
        public Post()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.Now;
        }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Author { get; set; }
    }
}