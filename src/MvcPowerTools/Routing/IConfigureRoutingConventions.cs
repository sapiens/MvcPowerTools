using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcPowerTools.Routing
{
    public interface IConfigureRoutingConventions
    {
        IConfigureAction If(Predicate<ActionCall> predicate);
        
        IConfigureRoutingConventions Add(IBuildRoutes convention);
        IConfigureRoutingConventions Add(IModifyRoute convention);

        IConfigureRoutingConventions Constrain<T>(Func<IRouteConstraint> factory);
        IConfigureRoutingConventions HomeIs<T>(Expression<Action<T>> actionSelector) where T:Controller;
        //IConfigureRoutingConventions DefaultBuilder(Func<ActionCall,IEnumerable<Route>> builder);
    }
}