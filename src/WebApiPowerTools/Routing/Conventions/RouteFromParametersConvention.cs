using System.Reflection;
using System.Text;
using System.Web.Http;
using System.Web.Http.Routing;

namespace WebApiPowerTools.Routing.Conventions
{
    public class RouteFromParametersConvention : RoutingConventionsModule
    {
    
        public override void Configure(IConfigureRoutingConventions routing)
        {
            routing.Always().Build(builderHelper =>
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
                return new[] {route};
            });
        }

        private static IHttpRouteConstraint GetConstraint(RouteBuilderInfo builderHelper,string arg)
        {
            ParameterInfo parameter = null;
            var argument = builderHelper.ActionCall.GetActionArgument(arg);
            if (argument!=null)
            {
                return builderHelper.GetConstraint(parameter.ParameterType);
            }
            return null;
        }

        static string GenerateRouteTemplate(RouteBuilderInfo builder)
        {
            var sb=new StringBuilder();
            sb.Append(builder.ActionCall.Controller.ControllerNameWithoutSuffix().ToLower())
                .Append("/")
                .Append(builder.ActionCall.Method.Name.ToLower())
                ;

            foreach (var arg in builder.ActionCall.GetArgumentNames())
            {
                sb.Append("/");
                sb.Append("{" + arg + "}");
            }
            return sb.ToString();
        }
    }
}