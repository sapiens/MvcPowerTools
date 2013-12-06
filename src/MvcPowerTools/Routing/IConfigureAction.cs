using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace MvcPowerTools.Routing
{
    public interface IConfigureAction
    {
        IConfigureRoutingConventions Build(Func<ActionCall, IEnumerable<Route>> builder);
        IConfigureRoutingConventions Modify(Action<Route,ActionCall> modifier);
    }
}