namespace MvcPowerTools.Routing
{
    public interface IMatchAction
    {
        bool Match(ActionCall action);
    }
}