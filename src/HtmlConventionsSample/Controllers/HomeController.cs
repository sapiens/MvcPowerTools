using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HtmlConventionsSample.Models;
using MvcPowerTools.ControllerHandlers;
using Ploeh.AutoFixture;

namespace HtmlConventionsSample.Controllers
{

    public class IndexIn {}

    public class IndexQuery : IHandleQueryAsync<IndexIn, FluentMyModel>
    {
        public Task<FluentMyModel> HandleAsync(IndexIn input)
        {
            var task = new Task<FluentMyModel>(()=> { 
                var f = new Fixture();
                var mode = f.Create<FluentMyModel>();
                mode.Name = "From query handler";
                Thread.Sleep(1000);
                return mode;               

            });
            task.Start();
            return task;
        }
    }

    public class QueryController :QueryControllerAsync<IndexIn,FluentMyModel>
    {
        
        public override async Task<ActionResult> Get(IndexIn input)
        {
            return await HandleAsync(input);
        }

        protected override ActionResult GetView(FluentMyModel model)
        {
            return View("index", model);
        }
    }
    //public class QueryController : Controller
    //{
    //    public ActionResult Index(IndexIn model)
    //    {
    //        // if (model==null) model=new IndexIn();
    //        return View("Index", (model ?? new IndexIn()).QueryTo<FluentMyModel>());
    //    }
    //}

  public class HomeController :Controller
    {
        public ActionResult Index()
        {
            var f = new Fixture();
            return View(f.Create<FluentMyModel>());
        }

        [HttpPost]
        public ActionResult Index(FluentMyModel model)
        {
            if (ModelState.IsValid)
            {
                return Content("ok");
            }
            return Content("not ok");

            //return View(model);
        }

        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            var fixture = new Fixture();
            var model=fixture.Build<MyModel>().Without(d => d.File).Create<MyModel>();
            return View(model);
        }

        public ActionResult Contact(string bla="lol")
        {
            ViewBag.Message = "Your contact page."+bla;

            return View();
        }
    }
}