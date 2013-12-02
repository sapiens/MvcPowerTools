using System;

namespace MvcPowerTools.Filters
{
    public interface IConfigureAction
    {
        IConfigureFilters Use(Func<object> factory);
        /// <summary>
        /// It will use the DependecyResolver to create the filter instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configure"></param>
        /// <returns></returns>
        IConfigureFilters Use<T>(Action<T> configure=null);
    }
}