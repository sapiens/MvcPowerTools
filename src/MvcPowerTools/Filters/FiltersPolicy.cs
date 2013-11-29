using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Reflection;
using System.Web.Mvc;

namespace MvcPowerTools.Filters
{
    public class FiltersPolicy
    {
        public FiltersPolicy()
        {
            Policies = new List<IFilterPolicy>();
            Actions=new List<MethodInfo>();
        }

        private static FiltersPolicy _instance=new FiltersPolicy();
        public static FiltersPolicy Config
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the list of policies to apply
        /// </summary>
        public IList<IFilterPolicy> Policies { get; private set; }
        /// <summary>
        /// Gets the list of actions which will be policies subjects
        /// </summary>
        public IList<MethodInfo> Actions { get; private set; }

        /// <summary>
        /// Builds the filter provider
        /// </summary>
        /// <returns></returns>
        public IFilterProvider BuildProvider()
        {
            List<FilterActionInfo> list=new List<FilterActionInfo>();
            
            foreach (var action in Actions)
            {
                var filters = new List<Filter>();
                foreach (var policy in Policies.Where(p => p.Match(action)))
                {
                    if (policy.Instance == null)
                    {
                        throw new InstanceNotFoundException("Filter instance is missing. Every filter policy needs an instance of the filter");
                    }
                    var f = new Filter(policy.Instance, FilterScope.Global,policy.Order);
                    filters.Add(f);
                }
                if (filters.Count > 0)
                {
                    list.Add(new FilterActionInfo(action,filters));
                }
            }

            return new FiltersWithPoliciesProvider(list);
        }

        /// <summary>
        /// Builds and registers the provider as a global filter provider
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="removeGlobal">True to remove the implicit GlobalFilters provider</param>
        public void RegisterProvider(FilterProviderCollection filters,bool removeGlobal=false)
        {
            if (removeGlobal)
            {
                filters.RemoveAll(d=>d.GetType()==typeof(GlobalFilterCollection));
            }
            filters.Add(BuildProvider());
        }
        
    }
}