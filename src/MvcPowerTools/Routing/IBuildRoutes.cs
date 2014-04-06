using System.Collections.Generic;
#if WEBAPI
using System.Web.Http.Routing;
#else
using System.Web.Routing;
#endif


#if WEBAPI
namespace WebApiPowerTools.Routing{
#else
namespace MvcPowerTools.Routing{    
#endif

   public interface IBuildRoutes:IMatchAction
    {
#if WEBAPI
    IEnumerable<IHttpRoute> Build(RouteBuilderInfo info);
#else
    IEnumerable<Route> Build(RouteBuilderInfo info);
#endif

    }
}