using System;
using System.Linq;
using CavemanTools;
using CavemanTools.Model;
using HtmlConventionsSample.Browse.Posts.ViewModels;
using HtmlConventionsSample._Auxilia.Data;
using MvcPowerTools.ControllerHandlers;


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
            model.Data.Items = _db.Posts.OrderByDescending(d => d.CreatedOn).Paginate(pagination).ToArray();
            model.Data.Count = _db.Posts.Count();
            return model;
        }
    }
}