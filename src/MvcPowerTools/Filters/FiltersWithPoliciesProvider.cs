using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcPowerTools.Filters
{
    internal class FiltersWithPoliciesProvider:IFilterProvider
    {
        private Dictionary<string,IEnumerable<Filter>> _filters=new Dictionary<string, IEnumerable<Filter>>();
        public FiltersWithPoliciesProvider(IEnumerable<FilterActionInfo> filters)
        {
            foreach (var filter in filters)
            {
                Filters.Add(filter.Key,filter.Filters);
            }
        }

        public Dictionary<string, IEnumerable<Filter>> Filters
        {
            get { return _filters; }
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
            if (!Filters.TryGetValue(key, out filters))
            {
                filters=new Filter[0];
            }
            return filters;
        }
    }
}