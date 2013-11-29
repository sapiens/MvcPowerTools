using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Routing;

namespace MvcPowerTools.Routing
{
    /// <summary>
    /// Handler convention, the controller contains 1 GET and 1 POST
    /// All GET methods should be like get(param|param=value). IF param hasn't a default value it's considered required.
    /// A parameter with default value of it type, it's considered optional
    /// POST method should be named just 'post'
    /// </summary>
    public class HandlerRouteConvention:IRouteConvention
    {
      
        public virtual bool Match(ActionCall actionCall)
        {
            return true;
        }

        public virtual IEnumerable<Route> Build(ActionCall action)
        {
            
            var sb = new StringBuilder();
            var name = action.Controller.Name;
            if (name.EndsWith("Controller", StringComparison.InvariantCultureIgnoreCase))
            {
                name = name.Substring(0, name.Length - 10);
            }
            sb.Append(name.ToLowerInvariant());
            var defaults = action.CreateDefaults();
            var constraints = new RouteValueDictionary();
            
            if (action.Method.Name.StartsWith("get", StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (var param in action.Arguments.Keys)
                {
                    sb.Append("/{").Append(param).Append("}");
                }
                action.SetParamsDefaults(defaults);
            }


            var httpMethod = action.Method.Name.StartsWith("post",
                                                           StringComparison.InvariantCultureIgnoreCase)
                                 ? "POST"
                                 : "GET";
            constraints["httpMethod"] =new HttpMethodConstraint(httpMethod) ;
            return new[]{ new Route(sb.ToString(),defaults,constraints,action.Settings.CreateHandler())};
        }
    }
}