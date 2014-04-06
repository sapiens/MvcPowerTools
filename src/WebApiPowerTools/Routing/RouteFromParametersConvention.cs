using System.Reflection;
using System.Text;
using System.Web.Http;
using System.Web.Http.Routing;

namespace WebApiPowerTools
{
    public class RouteFromParametersConvention : HttpRoutingConventionModule
    {
    
        public override void Configure(IConfigureHttpRoutingConventions routing)
        {
            routing.Always().Build(builderHelper =>
            {

                var route = builderHelper.ActionCall.CreateRoute(GenerateRouteName(builderHelper));
                
                builderHelper.ActionCall.SetParamsDefaults(route.Defaults);
                foreach (var arg in route.Defaults.Keys)
                {
                    IHttpRouteConstraint constraint = GetConstraint(builderHelper, arg);
                    if (constraint != null)
                    {
                        route.Constraints[arg] = constraint;
                    }
                }

                MethodInfo action = builderHelper.ActionCall.Action;
                if (action.Name.StartsWith("Get") || action.Name.StartsWith("Find") ||
                    action.GetCustomAttribute<HttpGetAttribute>() != null)
                {
                    route.ConstrainToGet();
                }

                if (action.Name.StartsWith("Post") || action.GetCustomAttribute<HttpPostAttribute>() != null)
                {
                    route.ConstrainToPost();
                }
                return new[] {route};
            });
        }

        private static IHttpRouteConstraint GetConstraint(RouteBuilderHelper builderHelper,string arg)
        {
            ParameterInfo parameter = null;
            if (builderHelper.ActionCall.Arguments.TryGetValue(arg, out parameter))
            {
                return builderHelper.GetConstraint(parameter.ParameterType);
            }
            return null;
        }

        static string GenerateRouteName(RouteBuilderHelper builder)
        {
            var sb=new StringBuilder();
            sb.Append(builder.ActionCall.GetControllerName().ToLower())
                .Append("/")
                .Append(builder.ActionCall.Action.Name.ToLower())
                ;

            foreach (var arg in builder.ActionCall.Arguments.Keys)
            {
                sb.Append("/");
                sb.Append("{" + arg + "}");
            }
            return sb.ToString();
        }
    }
}