using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
#if WEBAPI
using System.Web.Http.Routing;
#else
using System.Web.Mvc;
using System.Web.Routing;
using MvcPowerTools.Extensions;
#endif

#if WEBAPI
namespace System.Web.Http
#else
namespace System.Web.Mvc
#endif
{
    public static class ControllerExtensions
    {
#if !WEBAPI

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
        public static ActionResult RedirectToController<T>(this Controller c, Expression<Action<T>> selector, object routeValues = null) where T : Controller
        {
            return new RedirectToRouteResult(ToRouteValues(selector,routeValues));
        }

        public static string GetControllerName(this ControllerContext ctrl)
        {
            return ctrl.RouteData.GetRequiredString("controller");
        }

        public static void Add(this RouteValueDictionary dic, object value)
        {
            if (value == null) return;
            foreach (PropertyDescriptor p in TypeDescriptor.GetProperties(value))
            {
                dic.Add(p.Name, p.GetValue(value));
            }
        }

        internal static RouteValueDictionary ToRouteValues<T>(Expression<Action<T>> selector,object routeValues=null) where T : Controller
        {
            var method = selector.Body as MethodCallExpression;
            method.MustComplyWith(m => m != null, "Only controller actions are accepted");

            var action = method.Method.Name;
            var args = method.Arguments.ToArray();
            var methodParams = method.Method.GetParameters();

            RouteValueDictionary rv = new RouteValueDictionary(routeValues);
            var i = 0;
            foreach (var a in args)
            {
                var arg = a.GetValue();
                var info = methodParams[i];
                if (info.ParameterType.IsUserDefinedClass())
                {
                    rv.Add(arg);
                }
                else
                {
                    rv[info.Name.ToLower()] = arg;
                }
                
                i++;
            }
                    
            rv["action"] = action;
            rv["controller"] = typeof(T).ControllerNameWithoutSuffix();
            
            return rv;
        }

        public static ViewResult CreateView(this Controller controller, string viewName = null)
        {
            var ViewName = viewName ?? controller.GetActionName();
            var result = new ViewResult();
            result.TempData = controller.TempData;
            result.ViewData = controller.ViewData;
            result.ViewName = ViewName;
            return result;
        }

        /// <summary>
        /// Gets the invoked action for controller
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static string GetActionName(this ControllerContext ctrl)
        {
            if (ctrl == null) throw new ArgumentNullException("ctrl");
            return ctrl.RouteData.GetRequiredString("action");
        }
        
        /// <summary>
        /// Gets the invoked action for controller
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static string GetActionName(this Controller ctrl)
        {
            return ctrl.ControllerContext.GetActionName();            
        }
#endif

        public static string ControllerNameWithoutSuffix(this Type type)
        {
            var cname = type.Name;
            var cidx = cname.IndexOf("Controller");
            if (cidx > -1)
            {
                cname = cname.Remove(cidx, 10);
            }
            return cname;
        }

        public static IEnumerable<MethodInfo> GetActionMethods(this Type controllerType)
        {
            return
                controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance|BindingFlags.DeclaredOnly)
                    .Where(m => !m.HasCustomAttribute<NonActionAttribute>() /*&& !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_")*/);
        }

        /// <summary>
        /// Returns all public types inheriting Controller
        /// </summary>
        /// <param name="asm"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetControllerTypes(this Assembly asm)
        {
            asm.MustNotBeNull();
#if WEBAPI
            return asm.GetTypesDerivedFrom<ApiController>(true);
#else
		 return asm.GetTypesDerivedFrom<Controller>(true);
#endif
        }
            

       

    }
}