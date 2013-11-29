using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HtmlConventionsSample.Models;
using Ploeh.AutoFixture;

namespace HtmlConventionsSample.Controllers
{
    public class HomeController : Controller
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
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
          
            return View();
        }

        public ActionResult Contact(string bla="lol")
        {
            ViewBag.Message = "Your contact page."+bla;

            return View();
        }
    }
}