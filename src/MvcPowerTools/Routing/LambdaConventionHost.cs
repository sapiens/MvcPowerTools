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

        public Func<ActionCall, IEnumerable<Route>> Builder { get; set; }
        public Action<Route,ActionCall> Modifier { get; set; }

        public void Modify(Route route, ActionCall actionCall)
        {
            if (Modifier != null)
            {
                Modifier(route, actionCall);
            }
        }

        public IEnumerable<Route> Build(ActionCall actionInfo)
        {
            if (Builder != null)
            {
                return Builder(actionInfo);
            }
            return new Route[0];
        }
    }
}