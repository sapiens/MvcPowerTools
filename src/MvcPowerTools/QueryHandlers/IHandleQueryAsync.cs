using System.Threading.Tasks;

namespace MvcPowerTools.QueryHandlers
{
    public interface IHandleQueryAsync<in TInput, TOutput> where TInput:class where TOutput:class
    {
        Task<TOutput> HandleAsync(TInput input);
    }
}