using System;
using System.Web.Mvc;
using CavemanTools.Model;
using HtmlConventionsSample.Browse.Posts;
using HtmlConventionsSample.Edit.ViewModels;
using MvcPowerTools.ControllerHandlers;

namespace HtmlConventionsSample.Edit
{
    public class AddPostController : CommandController<AddPostModel,NoResult>
    {
        public ActionResult Get()
        {
            return View(new AddPostModel());
        }

        public override ActionResult Post(AddPostModel model)
        {
            if(!ModelState.IsValid) throw new Exception("[ValidModelOnly] should have kicked in");
            return this.Handle(model, nr => this.RedirectToController<PostsListController>(c => c.Get(new PagedInput())));
        }
    }
}