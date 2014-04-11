using HtmlConventionsSample.Edit.ViewModels;
using HtmlConventionsSample._Auxilia.Data;
using HtmlConventionsSample._Auxilia.Persistence;
using MvcPowerTools.ControllerHandlers;

namespace HtmlConventionsSample._Auxilia.Handlers
{
    public class AddPostHandler:IHandleCommand<AddPostModel,NoResult>
    {
        private readonly InMemoryDb _db;

        public AddPostHandler(InMemoryDb db)
        {
            _db = db;
        }

        public NoResult Handle(AddPostModel input)
        {
            var post = new Post();
            post.Title = input.Title;
            post.Author = input.Author;
            _db.Save(post);
            return NoResult.Instance;
        }
    }
}