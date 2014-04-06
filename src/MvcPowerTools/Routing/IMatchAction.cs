#if WEBAPI
namespace WebApiPowerTools.Routing
#else
namespace MvcPowerTools.Routing    
#endif
{
    public interface IMatchAction
    {
        bool Match(ActionCall action);
    }
}