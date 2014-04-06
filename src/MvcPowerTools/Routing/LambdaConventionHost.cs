using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace MvcPowerTools.Routing
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

        public Func<RouteBuilderInfo, IEnumerable<Route>> Builder { get; set; }
        public Action<Route, RouteBuilderInfo> Modifier { get; set; }

        public void Modify(Route route, RouteBuilderInfo info)
        {
            if (Modifier != null)
            {
                Modifier(route, info);
            }
        }

        public IEnumerable<Route> Build(RouteBuilderInfo info)
        {
            if (Builder != null)
            {
                return Builder(info);
            }
            return new Route[0];
        }
    }
}