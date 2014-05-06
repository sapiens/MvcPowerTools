using System;
using CavemanTools.Model;
using MvcPowerTools.Html;

namespace HtmlConventionsSample._Auxilia.Data
{
    public class Post
    {
        public Post()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.Now;
            Template=new SomeTemplate();
            Template.Id = new Random().Next();
        }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Author { get; set; }
        [DisplayTemplate]
        public SomeTemplate Template { get; set; }
    }

    public class SomeTemplate:IdName
    {
        
    }
}