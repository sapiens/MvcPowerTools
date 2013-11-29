namespace MvcPowerTools.Routing
{
    public interface IRouteUrlFormatPolicy:IMatchAction
    {
        string Format(string url,ActionCall actionInfo);
    }
}