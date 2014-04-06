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
    public interface IModifyRoute : IMatchAction
    {
#if WEBAPI
        void Modify(IHttpRoute route, RouteBuilderInfo info);
#else
        void Modify(Route route, RouteBuilderInfo info);
#endif
    }
}