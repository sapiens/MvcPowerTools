using System.Collections.Generic;
using System.Web.Routing;

namespace MvcPowerTools.Routing
{
   public interface IBuildRoutes:IMatchAction
    {
        IEnumerable<Route> Build(ActionCall actionInfo);
    }
}