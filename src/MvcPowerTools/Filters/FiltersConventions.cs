using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Reflection;
using System.Web.Mvc;

namespace MvcPowerTools.Filters
{
    public class FiltersConventions:IConfigureFilters
    {
        public FiltersConventions()
        {
            Conventions = new List<IFilterConvention>();
            Actions=new List<MethodInfo>();
        }

        private static FiltersConventions _instance=new FiltersConventions();
        public static FiltersConventions Config
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the list of conventions to apply
        /// </summary>
        public IList<IFilterConvention> Conventions { get; private set; }
        /// <summary>
        /// Gets the list of actions which will be conventions subjects
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
                foreach (var policy in Conventions.Where(p => p.Match(action)))
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

        public void LoadModule(params FiltersConventionsModule[] modules)
        {
            foreach (var module in modules)
            {
                module.Configure(this);
            }
        }

        class LambdaConfigurator:IConfigureAction
        {
            private readonly FiltersConventions _parent;
            private LambdaHostConvention _lambda;

            public LambdaConfigurator(FiltersConventions parent,Predicate<MethodInfo> predicate)
            {
                _parent = parent;
                _lambda = new LambdaHostConvention(predicate);
            }

            public IConfigureFilters Use(Func<object> factory)
            {
                _lambda.Instance = factory();
                _parent.Conventions.Add(_lambda);
                return _parent;
            }

            /// <summary>
            /// It will use the DependecyResolver to create the filter instance
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="configure"></param>
            /// <returns></returns>
            public IConfigureFilters Use<T>(Action<T> configure = null)
            {
                var inst = DependencyResolver.Current.GetService<T>();
                if (inst == null)
                {
                    throw new InvalidOperationException("Can't create an instance of {0}. Maybe you forgot to add it to the IoC Container?".ToFormat(typeof(T)));
                }
                if (configure != null)
                {
                    configure(inst);
                }
                _lambda.Instance = inst;
                _parent.Conventions.Add(_lambda);
                return _parent;
            }
        }

        public IConfigureAction If(Predicate<MethodInfo> match)
        {
           match.MustNotBeNull();
            return new LambdaConfigurator(this,match);
        }
    }
}