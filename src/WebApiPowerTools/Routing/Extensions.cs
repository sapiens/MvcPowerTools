using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Routing;

namespace WebApiPowerTools
{
    public static class Extensions
    {

        /// <summary>
        /// Creates a route value dictionary with controller and action values set
        /// </summary>
        /// <returns></returns>
        public static HttpRouteValueDictionary CreateDefaults(this HttpActionCall actionCall)
        {
            var defaults = new HttpRouteValueDictionary();
            defaults["controller"] = actionCall.GetControllerName();
            var name = actionCall.Action.Name;
            var actionAttrib = actionCall.Action.GetCustomAttribute<ActionNameAttribute>();
            if (actionAttrib != null)
            {
                name = actionAttrib.Name;
            }
            defaults["action"] = name;
            return defaults;
        }


        /// <summary>
        /// Creates a Route with no url pattern and with the defaults (controller,action) set
        /// </summary>
        /// <returns></returns>
        public static IHttpRoute CreateRoute(this HttpActionCall actionCall, string url = null)
        {
            return new HttpRoute(url,actionCall.CreateDefaults());
        }

        public static object GetDefault(this Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);
            else
                return (object)null;
        }

        /// <summary>
        /// Checks if type is a reference but also not
        ///              a string, array, Nullable, enum
        /// 
        /// </summary>
        /// <param name="type"/>
        /// <returns/>
        public static bool IsUserDefinedClass(this Type type)
        {
            return type.IsClass && Type.GetTypeCode(type) == TypeCode.Object && (!type.IsArray && !System.Net.Http.TypeExtensions.IsNullable(type));
        }

        /// <summary>
        /// Sets the default values for the route params. Only action parameters with default values are considered.
        /// If the value is equal to the type's default value, it's considered optional
        /// User defined class params are ignored.
        /// This method should not be used for POST.
        /// </summary>
        /// <param name="actionCall"></param>
        /// <param name="defaults"></param>
        public static void SetParamsDefaults(this HttpActionCall actionCall, IDictionary<string, object> defaults)
        {
            foreach (var p in actionCall.Arguments.Values)
            {
                if (!p.HasDefaultValue) continue;
                //if (p.RawDefaultValue == DBNull.Value) continue;
                
                if (p.RawDefaultValue == p.ParameterType.GetDefault())
                {
                    defaults[p.Name] = RouteParameter.Optional;
                }
                else
                {
                    defaults[p.Name] = p.RawDefaultValue;
                }
            }
        }

        /// <summary>
        /// Gets the controller name without the "Controler" suffix
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetControllerName(this HttpActionCall action)
        {
            string name = action.ControllerType.Name;
            if (name.EndsWith("Controller"))
            {
                return name.Substring(0, name.Length - 10);
            }
            return name;
        }

        public static IEnumerable<Type> GetHttpControllerTypes(this Assembly asm)
        {
            return asm.GetExportedTypes().Where(t => t.IsAbstract && typeof (ApiController).IsAssignableFrom(t));
        }

        public static IEnumerable<MethodInfo> GetActionMethods(this Type controllerType)
        {
            return
                controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(m => m.GetCustomAttribute<NonActionAttribute>() != null);
        }

        /// <summary>
        /// Register types deriving from ApiController class
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="asm"></param>
        public static HttpRoutingConventions RegisterControllers(this HttpRoutingConventions policy, Assembly asm)
        {
            return RegisterControllers(policy, asm.GetHttpControllerTypes().ToArray());
        }
        /// <summary>
        /// Register as actions types matching a criteria
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="asm"></param>
        /// <param name="match"></param>
        public static HttpRoutingConventions RegisterControllers(this HttpRoutingConventions policy, Assembly asm, Func<Type, bool> match)
        {
            RegisterControllers(policy, asm.GetTypes().Where(match).ToArray());
            return policy;
        }

        /// <summary>
        /// Registers actions from controller
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="policy"></param>
        public static HttpRoutingConventions RegisterController<T>(this HttpRoutingConventions policy) where T : ApiController
        {
            policy.RegisterControllers(typeof(T));
            return policy;
        }

        /// <summary>
        /// Registers actions from the provided types.
        /// All types should inherit from ApiControllers
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="controllers"></param>
        public static HttpRoutingConventions RegisterControllers(this HttpRoutingConventions policy, params Type[] controllers)
        {
            foreach (var c in controllers)
            {
                foreach (var action in c.GetActionMethods())
                {
                    policy.Actions.Add(new HttpActionCall(action));
                }
                
            }
            return policy;
        }

    
        public static HttpRoute Duplicate(this IHttpRoute route)
        {
            return new HttpRoute(route.RouteTemplate,new HttpRouteValueDictionary(route.Defaults),new HttpRouteValueDictionary(route.Constraints),new HttpRouteValueDictionary(route.DataTokens),route.Handler);
        }

        public static void ConstrainToGet(this IHttpRoute route)
        {
            route.Constrain("GET");
        }


        public static void ConstrainToPost(this IHttpRoute route)
        {
            route.Constrain("POST");
        }

        static void Constrain(this IHttpRoute route, string method)
        {
            route.Constraints["httpMethod"] = new HttpMethodConstraint(new HttpMethod(method));
        }

        public static IBuildHttpRoute Always(this IConfigureHttpRoutingConventions cfg)
        {
            return cfg.If(d => true);
        }
    }
}