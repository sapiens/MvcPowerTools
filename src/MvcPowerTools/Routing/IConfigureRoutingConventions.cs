using System;
using System.Collections.Generic;
using System.Linq.Expressions;
#if WEBAPI
using System.Web.Http.Routing;

#else
using System.Web.Mvc;
using System.Web.Routing;
#endif

#if WEBAPI
namespace WebApiPowerTools.Routing
#else
namespace MvcPowerTools.Routing
#endif
{
    public interface IConfigureRoutingConventions
    {
        IConfigureAction If(Predicate<ActionCall> predicate);
        
        IConfigureRoutingConventions Add(IBuildRoutes convention);
        IConfigureRoutingConventions Add(IModifyRoute convention);

#if WEBAPI
        IConfigureRoutingConventions Constrain<T>(Func<IHttpRouteConstraint> factory);
#else
        IConfigureRoutingConventions Constrain<T>(Func<IRouteConstraint> factory);
#endif
#if !WEBAPI
        IConfigureRoutingConventions HomeIs<T>(Expression<Action<T>> actionSelector) where T:Controller;
#endif

        //IConfigureRoutingConventions DefaultBuilder(Func<ActionCall,IEnumerable<Route>> builder);
    }
}