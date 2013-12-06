using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using HtmlConventionsSample.Controllers;
using MvcPowerTools.Routing;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;

namespace HtmlConventionsSample
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RoutingConventions.Configure(r =>
            {
                r
                    .RegisterController<HomeController>()
                    .HomeIs<HomeController>(h => h.Index())
                    .DefaultBuilder(a =>
                    {
                        var url = a.Method.Name;
                        var route = a.CreateRoute(url);
                        if (a.Method.HasCustomAttribute<HttpPostAttribute>())
                        {
                            route.Constraints["method"] = new HttpMethodConstraint("POST");
                        }
                      return new[] {route};
                    });

            });
        }
    }
}
