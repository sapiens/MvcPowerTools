using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HtmlConventionsSample.Browse.Posts;
using HtmlConventionsSample.Edit.ViewModels;
using HtmlConventionsSample._Auxilia;
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            return this.RedirectToController<PostsListController>(c => c.Get(new PagedInput()));
        }
    }
}