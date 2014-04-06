#if WEBAPI

namespace WebApiPowerTools.Routing
#else
namespace MvcPowerTools.Routing
#endif
{
    public abstract class RoutingConventionsModule
    {
        public abstract void Configure(IConfigureRoutingConventions conventions);
    }
}