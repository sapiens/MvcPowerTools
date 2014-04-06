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
    public interface IConfigureAction
    {
#if WEBAPI
        IConfigureRoutingConventions Build(Func<RouteBuilderInfo, IEnumerable<IHttpRoute>> builder);
        IConfigureRoutingConventions Modify(Action<IHttpRoute, RouteBuilderInfo> modifier);
#else
        IConfigureRoutingConventions Build(Func<RouteBuilderInfo, IEnumerable<Route>> builder);
        IConfigureRoutingConventions Modify(Action<Route, RouteBuilderInfo> modifier);
#endif
    }
}