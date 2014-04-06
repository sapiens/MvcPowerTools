using System;
using System.Web.Http.Routing;

namespace WebApiPowerTools
{
    public interface IBuildHttpRoute
    {
        IConfigureHttpRoutingConventions Build(Func<RouteBuilderHelper, IHttpRoute[]> builder);
    }
}