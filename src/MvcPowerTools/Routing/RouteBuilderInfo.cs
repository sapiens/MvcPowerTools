using System;

#if WEBAPI
using System.Web.Http;
using System.Web.Http.Routing;
#else 
using System.Web.Routing;
using System.Web.Mvc;
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
#if WEBAPI
        public HttpRouteValueDictionary CreateDefaults() 
#else
		public RouteValueDictionary CreateDefaults()
#endif
        {
#if WEBAPI
            var defaults = new HttpRouteValueDictionary();
#else
		    var defaults = new RouteValueDictionary();
#endif
            var controler = ActionCall.Controller.ControllerNameWithoutSuffix();
            defaults["controller"] = controler;
            defaults["action"] = ActionCall.Method.Name;
            return defaults;
        }


        /// <summary>
        /// Creates a Route with no url pattern and with the defaults (controller,action) set
        /// </summary>
        /// <returns></returns>
#if WEBAPI
        public IHttpRoute CreateRoute(string url = ActionCall.EmptyRouteUrl)
        {
            url.MustNotBeEmpty();
            return new HttpRoute(url, CreateDefaults());
        }
#else
		public Route CreateRoute(string url = ActionCall.EmptyRouteUrl)
        {
            url.MustNotBeEmpty();
            var r= new Route(url, CreateDefaults(), new RouteValueDictionary(), Settings.CreateHandler());
            r.DataTokens = new RouteValueDictionary();
            return r;
        }
#endif
        

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
 
#if WEBAPI
        public IHttpRouteConstraint GetConstraint<T>()
#else
        public IRouteConstraint GetConstraint<T>()
#endif
        
        {
            return GetConstraint(typeof(T));
        }
    }
    
}