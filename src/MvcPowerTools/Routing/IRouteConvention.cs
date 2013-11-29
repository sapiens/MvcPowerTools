using System.Collections.Generic;
using System.Web.Routing;

namespace MvcPowerTools.Routing
{
    /// <summary>
    /// How to generate routes from actions
    /// </summary>
    public interface IRouteConvention
    {
        bool Match(ActionCall actionCall);
        IEnumerable<Route> Build(ActionCall actionInfo);
    }
    
}