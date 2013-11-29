using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace MvcPowerTools.Routing
{
    public static class Extensions
    {
         public static string StripNamespaceRoot(this RoutingPolicySettings settings, string @namespace)
         {
             return @namespace.Remove(0, settings.NamespaceRoot.Length);
         }

        /// <summary>
        /// Gets the controller name without the "Controler" suffix
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetControllerName(this ActionCall action)
        {
            var name = action.Controller.Name;
            return name.Substring(0, name.Length - 10);
        }


        /// <summary>
        /// Scans assembly and registers policies. Uses dependency resolver
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="asm"></param>
        /// <param name="res">Null means it uses the current dependecy resolver</param>
        public static RoutingPolicy RegisterPolicies(this RoutingPolicy policy, Assembly asm,IDependencyResolver res=null)
        {
            if (res == null)
            {
                res = DependencyResolver.Current;
            }            
            asm.GetTypesDerivedFrom<IRouteConvention>(true).ForEach(t =>
                {
                    policy.Conventions.Add(res.GetService(t) as IRouteConvention);
                });
            
            asm.GetTypesDerivedFrom<IRouteUrlFormatPolicy>(true).ForEach(t =>
                {
                    policy.UrlFormatPolicies.Add(res.GetService(t) as IRouteUrlFormatPolicy);
                });
            
            asm.GetTypesDerivedFrom<IRouteGlobalPolicy>(true).ForEach(t =>
                {
                    policy.GlobalPolicies.Add(res.GetService(t) as IRouteGlobalPolicy);
                });

            return policy;
        }

        /// <summary>
        /// Register types deriving from Controller class
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="asm"></param>
        public static RoutingPolicy RegisterControllers(this RoutingPolicy policy,Assembly asm)
        {
            return Register(policy,asm,t =>t.DerivesFrom<Controller>());          
        }
        /// <summary>
        /// Register as actions types matching a criteria
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="asm"></param>
        /// <param name="match"></param>
        public static RoutingPolicy Register(this RoutingPolicy policy, Assembly asm, Func<Type, bool> match)
        {
            RegisterActions(policy, asm.GetTypes().Where(match).ToArray());
            return policy;
        }

        /// <summary>
        /// Registers actions from controller
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="policy"></param>
        public static RoutingPolicy Register<T>(this RoutingPolicy policy) where T : Controller
        {
            policy.RegisterActions(typeof(T));
            return policy;
        }

        /// <summary>
        /// Registers actions from the provided types.
        /// All types should be Controllers
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="controllers"></param>
        public static RoutingPolicy RegisterActions(this RoutingPolicy policy, params Type[] controllers)
        {
            foreach (var c in controllers)
            {
                c.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).ForEach(m =>
                {
                    policy.AddAction(new ActionCall(m, policy.Settings));
                }); 
            }
            return policy;
        }

        public static RoutingPolicy RegisterHandlerConvention(this RoutingPolicy policy)
        {
            policy.Conventions.Add(new HandlerRouteConvention());
            return policy;
        }

        public static RoutingPolicy ConfigureFrom(this RoutingPolicy policy,params Assembly[] asms)
        {
            foreach (var asm in asms)
            {
                policy.RegisterControllers(asm).RegisterPolicies(asm);
            }
            return policy;
        }
    }
}