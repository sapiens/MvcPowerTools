using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcPowerTools.Filters
{
    internal class FiltersWithPoliciesProvider:IFilterProvider
    {
        Dictionary<string,IEnumerable<Filter>> _filters=new Dictionary<string, IEnumerable<Filter>>();
        public FiltersWithPoliciesProvider(IEnumerable<FilterActionInfo> filters)
        {
            foreach (var filter in filters)
            {
                _filters.Add(filter.Key,filter.Filters);
            }
        }
        
        
        /// <summary>
        /// Returns an enumerator that contains all the <see cref="T:System.Web.Mvc.IFilterProvider"/> instances in the service locator.
        /// </summary>
        /// <returns>
        /// The enumerator that contains all the <see cref="T:System.Web.Mvc.IFilterProvider"/> instances in the service locator.
        /// </returns>
        /// <param name="controllerContext">The controller context.</param><param name="actionDescriptor">The action descriptor.</param>
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var key = FilterActionInfo.CreateKey(actionDescriptor.ControllerDescriptor.ControllerType,
                                                 actionDescriptor.ActionName);
            IEnumerable<Filter> filters;
            if (!_filters.TryGetValue(key, out filters))
            {
                filters=new Filter[0];
            }
            return filters;
        }
    }
}