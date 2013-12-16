namespace MvcPowerTools.QueryHandlers
{
    public interface IHandleQuery<in TInput, out TOutput> where TInput:class where TOutput:class
    {
        TOutput Handle(TInput input);
    }
}