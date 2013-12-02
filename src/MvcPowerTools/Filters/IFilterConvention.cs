using System.Reflection;

namespace MvcPowerTools.Filters
{
    /// <summary>
    /// Convention to apply filters
    /// </summary>
    public interface IFilterConvention
    {
        int? Order { get; }
        /// <summary>
        /// Filter instance
        /// </summary>
        object Instance { get; }
        
        bool Match(MethodInfo action);
    }
}