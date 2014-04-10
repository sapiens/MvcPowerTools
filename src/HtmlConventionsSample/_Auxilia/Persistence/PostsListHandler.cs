using System.Linq;
using CavemanTools.Model;
using HtmlConventionsSample.Browse.Posts.ViewModels;
using HtmlConventionsSample._Auxilia.Data;
using MvcPowerTools.ControllerHandlers;
using MvcPowerTools.Extensions;
using Ploeh.AutoFixture;

namespace HtmlConventionsSample._Auxilia.Persistence
{
    public class PostsListHandler:IHandleQuery<PagedInput,PostsListModel>
    {
        private readonly InMemoryDb _db;

        public PostsListHandler(InMemoryDb db)
        {
            _db = db;
        }

        public PostsListModel Handle(PagedInput input)
        {
            var model = new PostsListModel(input);
            var pagination = input.ToPagination();
            model.Data = new PagedResult<Post>();
            model.Data.Items=  _db.Posts.Skip((int) pagination.Skip).Take(pagination.PageSize).ToArray();
            model.Data.Count = _db.Posts.Count();
            return model;
        }
    }
}