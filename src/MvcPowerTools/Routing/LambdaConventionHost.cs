using System;
using System.Collections.Generic;
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
    class LambdaConventionHost : IBuildRoutes,IModifyRoute
    {
        private readonly Predicate<ActionCall> _predicate;

        public LambdaConventionHost(Predicate<ActionCall> predicate)
        {
            _predicate = predicate;
        }

        public bool Match(ActionCall action)
        {
            return _predicate(action);
        }

#if WEBAPI
        public Func<RouteBuilderInfo, IEnumerable<IHttpRoute>> Builder { get; set; }
        public Action<IHttpRoute, RouteBuilderInfo> Modifier { get; set; }
#else
		public Func<RouteBuilderInfo, IEnumerable<Route>> Builder { get; set; }
        public Action<Route, RouteBuilderInfo> Modifier { get; set; }
#endif

#if WEBAPI
        public void Modify(IHttpRoute route, RouteBuilderInfo info)
#else
		 public void Modify(Route route, RouteBuilderInfo info)
#endif
        {
            if (Modifier != null)
            {
                Modifier(route, info);
            }
        }

#if WEBAPI
        public IEnumerable<IHttpRoute> Build(RouteBuilderInfo info)
#else
		public IEnumerable<Route> Build(RouteBuilderInfo info)
#endif
        {
            if (Builder != null)
            {
                return Builder(info);
            }
#if WEBAPI
            return new IHttpRoute[0];
#else
		return new Route[0];
#endif
        }
    }
}