using System.Web.Routing;

namespace MvcPowerTools.Routing
{
    public interface IRouteGlobalPolicy
    {
        void ApplyTo(Route route);
    }
}