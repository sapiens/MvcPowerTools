using System.Web.Routing;

namespace MvcPowerTools.Routing
{
    public interface IModifyRoute : IMatchAction
    {
        void Modify(Route route, ActionCall actionCall);
    }
}