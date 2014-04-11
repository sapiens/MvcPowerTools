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
            var pagination = input.ToPagination(PagedInput.DefaultPageSize);
            model.Data = new PagedResult<Post>();
            model.Data.Items = _db.Posts.OrderByDescending(d => d.CreatedOn).Skip((int)pagination.Skip).Take(PagedInput.DefaultPageSize).ToArray();
            model.Data.Count = _db.Posts.Count();
            return model;
        }
    }
}