using System.ComponentModel.DataAnnotations;

namespace HtmlConventionsSample.Edit.ViewModels
{
    public class AddPostModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        public bool Check { get; set; }
    }
}