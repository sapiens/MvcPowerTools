using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace MvcPowerTools.Filters
{
    public static class Extensions
    {
         public static void RegisterControllers(this FiltersConventions conventions, params Type[] controllers)
         {
             foreach (var c in controllers)
             {
                 c.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).ForEach(m =>
                 {
                     conventions.Actions.Add(m);
                 });
             }        
         }

        public static void RegisterControllers(this FiltersConventions conventions, Assembly asm)
        {
            RegisterControllers(conventions,asm.GetTypesDerivedFrom<Controller>(true).ToArray());
        }

        public static void RegisterControllers<T>(this FiltersConventions conventions)
        {
            conventions.RegisterControllers(typeof(T));
        }

        /// <summary>
        /// Register as actions types matching a criteria
        /// </summary>
        /// <param name="conventions"></param>
        /// <param name="asm"></param>
        /// <param name="match"></param>
        public static void Register(this FiltersConventions conventions, Assembly asm, Func<Type, bool> match)
        {
            RegisterControllers(conventions, asm.GetTypes().Where(match).ToArray());
        }

        /// <summary>
        /// Scans assembly and registers all policies found.
        /// Uses dependecy resolver
        /// </summary>
        /// <param name="conventions"></param>
        /// <param name="asm"></param>
        public static void RegisterPolicies(this FiltersConventions conventions, Assembly asm)
        {
            var resolve = DependencyResolver.Current;
            asm.GetTypesDerivedFrom<IFilterConvention>()
                .Select(t => resolve.GetService(t))
                .Cast<IFilterConvention>()
                .ForEach(f =>
                    {
                        conventions.Conventions.Add(f);
                    });
        }
    }
}