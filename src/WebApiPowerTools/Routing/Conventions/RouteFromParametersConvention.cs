using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.Http;
using System.Web.Http.Routing;

namespace WebApiPowerTools.Routing.Conventions
{
    //todo make it use onemodelin
    public class RouteFromParametersConvention : IBuildRoutes
    {
        private static IHttpRouteConstraint GetConstraint(RouteBuilderInfo builderHelper,string arg)
        {
            var argument = builderHelper.ActionCall.GetActionArgument(arg);
            if (argument!=null)
            {
                return builderHelper.GetConstraint(argument.ParameterType);
            }
            return null;
        }

        protected virtual string FormatActionName(string name)
        {
            return name.ToLower();
        }

        string GenerateRouteTemplate(RouteBuilderInfo builder)
        {
            var sb=new StringBuilder();
            sb.Append(builder.ActionCall.Controller.ControllerNameWithoutSuffix().ToLower())
                .Append("/")
                .Append(FormatActionName(builder.ActionCall.Method.Name))
                ;

            foreach (var arg in builder.ActionCall.GetArgumentNames())
            {
                sb.Append("/");
                sb.Append("{" + arg + "}");
            }
            return sb.ToString();
        }

        public virtual bool Match(ActionCall action)
        {
            return true;
        }

        public IEnumerable<IHttpRoute> Build(RouteBuilderInfo builderHelper)
        {
            var route = builderHelper.CreateRoute(GenerateRouteTemplate(builderHelper));

            builderHelper.ActionCall.SetParamsDefaults(route.Defaults);
            foreach (var arg in builderHelper.ActionCall.GetArgumentNames())
            {
                IHttpRouteConstraint constraint = GetConstraint(builderHelper, arg);
                if (constraint != null)
                {
                    route.Constraints[arg] = constraint;
                }
            }

            MethodInfo action = builderHelper.ActionCall.Method;
            if (action.Name.StartsWith("Get") || action.Name.StartsWith("Find") ||
                action.GetCustomAttribute<HttpGetAttribute>() != null)
            {
                route.ConstrainToGet();
            }

            if (action.Name.StartsWith("Post") || action.GetCustomAttribute<HttpPostAttribute>() != null)
            {
                route.ConstrainToPost();
            }
            return new[] { route };
        }
    }
}