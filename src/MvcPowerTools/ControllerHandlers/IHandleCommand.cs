namespace MvcPowerTools.ControllerHandlers
{
    public interface IHandleCommand<TInput, TResult> : IHandleAction<TInput, TResult> where TResult : class
        where TInput : class
    {
        
    }
}