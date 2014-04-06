using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using MvcPowerTools.Routing.Conventions;

namespace MvcPowerTools.Routing
{
    public static class Extensions
    {

      
        /// <summary>
        /// Returns action name, taking into account [ActionName]
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetActionName(this ActionCall action)
        {
            var attrib = action.Method.GetCustomAttribute<ActionNameAttribute>();
            if (attrib != null) return attrib.Name;
            return action.Method.Name;
        }

        /// <summary>
        /// Sets the defaults for the route params. Only action parameters with default values are considered.
        /// If the value is equal to the type's default value, it's considered optional
        /// User defined class params are ignored.
        /// This method should not be used for POST.
        /// </summary>
        /// <param name="defaults"></param>
        public static void SetParamsDefaults(this ActionCall actionCall,IDictionary<string,object> defaults)
        {
            foreach (var p in actionCall.Method.GetParameters().Where(d=>!d.ParameterType.IsUserDefinedClass()))
            {
                if (p.RawDefaultValue == DBNull.Value) continue;

                if (p.RawDefaultValue == p.ParameterType.GetDefault())
                {
                    defaults[p.Name] = UrlParameter.Optional;
                }
                else
                {
                    defaults[p.Name] = p.RawDefaultValue;
                }
            }
        }

        /// <summary>
        /// Removes the namespace root from the provided string
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="namespace">Usually the namespace of the controller</param>
        /// <returns></returns>
        public static string StripNamespaceRoot(this RoutingConventionsSettings settings, string @namespace)
         {
             return @namespace.Remove(0, settings.NamespaceRoot.Length);
         }

        ///// <summary>
        ///// Gets the controller name without the "Controler" suffix
        ///// </summary>
        ///// <param name="action"></param>
        ///// <returns></returns>
        //public static string GetControllerName(this ActionCall action)
        //{
        //    var name = action.Controller.Name;
        //    return name.Substring(0, name.Length - 10);
        //}

        /// <summary>
        /// True if the action name starts with 'get'
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool IsGet(this ActionCall action)
        {
            return action.Method.Name.StartsWith("get", StringComparison.OrdinalIgnoreCase);
        }
        
        /// <summary>
        /// True if the action name starts with 'post'
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool IsPost(this ActionCall action)
        {
            return action.Method.Name.StartsWith("post", StringComparison.OrdinalIgnoreCase);
        }

        public static void LoadModules(this RoutingConventions policy, Assembly asm)
        {
            policy
                .LoadModule(
                asm.GetTypesDerivedFrom<RoutingConventionsModule>(true).Select(t => (RoutingConventionsModule)t.CreateInstance()).ToArray()
                );
        }

        /// <summary>
        /// Scans assembly and registers routing convetions but not configuration modules. Uses dependency resolver
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="asm"></param>
        /// <param name="res">Null means it uses the current dependecy resolver</param>
        public static RoutingConventions RegisterConventions(this RoutingConventions policy, Assembly asm, IDependencyResolver res = null)
        {
            if (res == null)
            {
                res = DependencyResolver.Current;
            }
            
            foreach(var builder in asm.GetTypesDerivedFrom<IBuildRoutes>())
            {
                policy.Add(res.GetInstance<IBuildRoutes>(builder));
            }
            
            foreach(var modifier in asm.GetTypesDerivedFrom<IModifyRoute>())
            {
                policy.Add(res.GetInstance<IModifyRoute>(modifier));
            }

            //policy.LoadModule(asm.GetTypesDerivedFrom<RoutingConventionsModule>(true).Select(t =>
            //{
            //    return res.GetInstance<RoutingConventionsModule>(t);
            //}).ToArray());
            
            return policy;
        }

        static T GetInstance<T>(this IDependencyResolver res,Type type) where T:class 
        {
            var inst = (T)res.GetService(type);
            if (inst == null)
            {
                inst = type.CreateInstance() as T;
            }
            return inst;
        }
        /// <summary>
        /// Searches routing conventions modules and registers them in Di Container
        /// </summary>
        /// <param name="asm"></param>
        /// <param name="containerRegister"></param>
        public static void RegisterModulesInContainer(this Assembly asm,
            Action<Type> containerRegister)
        {
            foreach (var type in asm.GetTypesDerivedFrom<RoutingConventionsModule>(true))
            {
                containerRegister(type);
            }            
        }

        /// <summary>
        /// Register types deriving from Controller class
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="asm"></param>
        public static RoutingConventions RegisterControllers(this RoutingConventions policy,Assembly asm)
        {
            return RegisterControllers(policy,asm.GetControllerTypes().ToArray());          
        }
        ///// <summary>
        ///// Register as actions types matching a criteria
        ///// </summary>
        ///// <param name="policy"></param>
        ///// <param name="asm"></param>
        ///// <param name="match"></param>
        //public static RoutingConventions RegisterController(this RoutingConventions policy, Assembly asm, Func<Type, bool> match)
        //{
        //    RegisterController(policy, asm.GetTypes().Where(match).ToArray());
        //    return policy;
        //}

        /// <summary>
        /// Registers actions from controller
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="policy"></param>
        public static RoutingConventions RegisterController<T>(this RoutingConventions policy) where T : Controller
        {
            policy.RegisterControllers(typeof(T));
            return policy;
        }

        /// <summary>
        /// Registers actions from the provided types.
        /// All types should be Controllers
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="controllers"></param>
        public static RoutingConventions RegisterControllers(this RoutingConventions policy, params Type[] controllers)
        {
            foreach (var c in controllers)
            {
                policy.Actions.AddRange(c.GetActionMethods().Select(m=>new ActionCall(m)));                
            }
            return policy;
        }

        public static RoutingConventions UseHandlerConvention(this RoutingConventions policy)
        {
            policy.Add(new HandlerRouteConvention());
            return policy;
        }

        /// <summary>
        /// This conventions applies only for GET requests
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public static RoutingConventions UseOneModelInHandlerConvention(this RoutingConventions policy,Predicate<ActionCall> applyTo=null)
        {
            policy.Add(new OneModelInHandlerConvention(applyTo));
            return policy;
        }

        public static Route Duplicate(this Route route)
        {
            return new Route(route.Url,new RouteValueDictionary(route.Defaults), new RouteValueDictionary(route.Constraints), route.RouteHandler);
        }

        public static void ConstrainToGet(this Route route)
        {
            route.Constrain("GET");
        }


        public static void ConstrainToPost(this Route route)
        {
            route.Constrain("POST");
        }

        static void Constrain(this Route route, string method)
        {
            route.MustNotBeNull();
            route.Constraints["httpMethod"] = new HttpMethodConstraint(method);
        }

        public static IConfigureAction Always(this IConfigureRoutingConventions cfg)
        {
            return cfg.If(d => true);
        }
    }
}