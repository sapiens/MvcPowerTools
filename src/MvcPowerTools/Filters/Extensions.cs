using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace MvcPowerTools.Filters
{
    public static class Extensions
    {
         public static void RegisterActions(this FiltersPolicy policy, params Type[] controllers)
         {
             foreach (var c in controllers)
             {
                 c.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).ForEach(m =>
                 {
                     policy.Actions.Add(m);
                 });
             }        
         }

        public static void RegisterControllers(this FiltersPolicy policy, Assembly asm)
        {
            RegisterActions(policy,asm.GetTypesDerivedFrom<Controller>(true).ToArray());
        }

        public static void RegisterActions<T>(this FiltersPolicy policy)
        {
            policy.RegisterActions(typeof(T));
        }

        /// <summary>
        /// Register as actions types matching a criteria
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="asm"></param>
        /// <param name="match"></param>
        public static void Register(this FiltersPolicy policy, Assembly asm, Func<Type, bool> match)
        {
            RegisterActions(policy, asm.GetTypes().Where(match).ToArray());
        }

        /// <summary>
        /// Scans assembly and registers all policies found.
        /// Uses dependecy resolver
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="asm"></param>
        public static void RegisterPolicies(this FiltersPolicy policy, Assembly asm)
        {
            var resolve = DependencyResolver.Current;
            asm.GetTypesDerivedFrom<IFilterPolicy>()
                .Select(t => resolve.GetService(t))
                .Cast<IFilterPolicy>()
                .ForEach(f =>
                    {
                        policy.Policies.Add(f);
                    });
        }
    }
}