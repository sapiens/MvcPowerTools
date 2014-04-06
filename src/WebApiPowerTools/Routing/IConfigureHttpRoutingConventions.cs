using System;
using System.Web.Http.Routing;

namespace WebApiPowerTools
{
    public interface IConfigureHttpRoutingConventions:ISelectAction
    {
        IConfigureHttpRoutingConventions LoadModule(params HttpRoutingConventionModule[] modules);
        IConfigureHttpRoutingConventions Constrain<T>(Func<IHttpRouteConstraint> constrainRule);
    }
}