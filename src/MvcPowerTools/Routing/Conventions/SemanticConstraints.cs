#if WEBAPI
using System.Net.Http;
namespace WebApiPowerTools.Routing.Conventions
#else
namespace MvcPowerTools.Routing.Conventions
#endif
{
    /// <summary>
    /// Automatically adds http constraints (Get, POST etc) to routes based on action name and attributes
    /// </summary>
    public class SemanticConstraints:RoutingConventionsModule
    {
        public override void Configure(IConfigureRoutingConventions conventions)
        {
            conventions.Always()
                .Modify((route, info) =>
                {
                    if (info.ActionCall.IsGet())
                    {
                        route.ConstrainToGet();
                        return;
                    }

                    if (info.ActionCall.IsPost())
                    {
                        route.ConstrainToPost();
                        return;
                    }

#if WEBAPI
                    if (info.ActionCall.IsPut())
                    {
                        route.Constrain(HttpMethod.Put);
                        return;
                    }
                    
                    if (info.ActionCall.IsDelete())
                    {
                        route.Constrain(HttpMethod.Delete);
                        return;
                    }
#endif
                });
        }
    }
}