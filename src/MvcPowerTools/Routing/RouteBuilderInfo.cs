using System;
using System.Reflection;
using System.Web.Mvc;
#if WEBAPI
using System.Web.Http.Routing;
#else 
using System.Web.Routing;
#endif


#if WEBAPI
namespace WebApiPowerTools.Routing
#else
namespace MvcPowerTools.Routing  
#endif
    
{
    public class RouteBuilderInfo
    {
        private readonly RoutingConventions _conventions;

        public RouteBuilderInfo(ActionCall call,RoutingConventions conventions)
        {
            _conventions = conventions;
            ActionCall = call;
        }

        public ActionCall ActionCall { get; private set; }

        public RoutingConventionsSettings Settings
        {
            get { return _conventions.Settings; }
        }

        /// <summary>
        /// Creates a route value dictionary with controller and action values set
        /// </summary>
        /// <returns></returns>
        public RouteValueDictionary CreateDefaults()
        {
            var defaults = new RouteValueDictionary();
            var controler = ActionCall.Controller.ControllerNameWithoutSuffix();
            defaults["controller"] = controler;
            //var actionAttrib = ActionCall.Method.GetCustomAttribute<ActionNameAttribute>();
            //if (actionAttrib != null)
            //{
            //    name = actionAttrib.Name;
            //}
            defaults["action"] = ActionCall.Method.Name;
            return defaults;
        }


        /// <summary>
        /// Creates a Route with no url pattern and with the defaults (controller,action) set
        /// </summary>
        /// <returns></returns>
        public Route CreateRoute(string url = ActionCall.EmptyRouteUrl)
        {
            url.MustNotBeEmpty();
            return new Route(url, CreateDefaults(), new RouteValueDictionary(), Settings.CreateHandler());
        }

        /// <summary>
        /// Gets defined constraint for type or null
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
#if WEBAPI
        public IHttpRouteConstraint GetConstraint(Type type)
#else
        public IRouteConstraint GetConstraint(Type type)
#endif
        
        {
            if (_conventions.Constraints.ContainsKey(type))
            {
                return _conventions.Constraints[type]();
            }
            return null;
        }
    }
}