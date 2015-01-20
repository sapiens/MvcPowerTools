using System.Collections.Generic;
using System.Web;
using System.Web.Routing;

namespace MvcPowerTools.Routing.Conventions
{
    public class NamespaceBasedRouting : IBuildRoutes
    {
        public virtual bool Match(ActionCall action)
        {
            return true;
        }

        public IEnumerable<Route> Build(RouteBuilderInfo info)
        {
            var url = info.ActionCall.Controller.ToWebsiteRelativePath(info.ActionCall.Controller.Assembly).ToLower();
            url = url.TrimStart('~', '/');
            var route = info.CreateRoute(url);
            route.DataTokens["Namespaces"] = new[] { info.ActionCall.Controller.Namespace };
            return new[] { route };
        }
    }
}