using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using MvcPowerTools.Extensions;

namespace System.Web.Mvc
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Redirects to the selected action
        /// </summary>
        /// <typeparam name="T">Controller</typeparam>
        /// <param name="ctrl">controller</param>
        /// <param name="selector">lambda statement</param>
        /// <returns></returns>
        public static ActionResult RedirectToAction<T>(this T ctrl,Expression<Action<T>> selector) where T:Controller
         {
            return new RedirectToRouteResult(ToRouteValues(selector));            
         }

        /// <summary>
        /// Redirects to the selected action from another controller
        /// </summary>
        /// <typeparam name="T">Controller class</typeparam>
        /// <param name="c"></param>
        /// <param name="selector">lambda statement</param>
        /// <returns></returns>
        public static ActionResult RedirectToController<T>(this Controller c,Expression<Action<T>> selector) where T:Controller
        {
            return new RedirectToRouteResult(ToRouteValues(selector));
        }
     
        /// <summary>
        /// Gets the invoked action for controller
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static string GetActionName(this Controller ctrl)
        {
            if (ctrl == null) throw new ArgumentNullException("ctrl");
            return ctrl.RouteData.GetRequiredString("action");
        }

        public static string GetControllerName(this ControllerContext ctrl)
        {
            return ctrl.RouteData.GetRequiredString("controller");
        }
     
       internal static RouteValueDictionary ToRouteValues<T>(Expression<Action<T>> selector) where T:Controller
       {
            var method = selector.Body as MethodCallExpression;
           var action = method.Method.Name;
           var args = method.Method.GetParameters();
            var param = method.Arguments.ToArray();
           
           var rv = new RouteValueDictionary();
           rv["action"] = action;
           var cname = typeof(T).Name;
           var cidx = cname.IndexOf("Controller");
           if (cidx > -1)
           {
               cname = cname.Remove(cidx, 10);
           }
           rv["controller"] = cname;
          
           foreach (var p in args)
           {
               rv[p.Name] = param[p.Position].GetValue();              
           }
           return rv;
       }

        public static ViewResult CreateView(this Controller controller,string viewName=null)
        {
            var ViewName = viewName??controller.GetActionName();
            var result = new ViewResult();
            result.TempData = controller.TempData;
            result.ViewData = controller.ViewData;
            result.ViewName = ViewName;            
            return result;
        }

    }
}