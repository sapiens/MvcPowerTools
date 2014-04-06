using System;
using System.Collections.Generic;
#if WEBAPI
using System.Web.Http.Routing;

#else
using System.Web.Routing;
#endif

#if WEBAPI

namespace WebApiPowerTools.Routing
#else
namespace MvcPowerTools.Routing
#endif
{
    class LambdaConventionConfigurator:IConfigureAction
    {
       
        private LambdaConventionHost _lambda;
        private RoutingConventions _parent;

        public LambdaConventionConfigurator(RoutingConventions parent,Predicate<ActionCall> predicate)
        {
            _parent = parent;
            _lambda = new LambdaConventionHost(predicate);
        }

#if WEBAPI
        public IConfigureRoutingConventions Build(Func<RouteBuilderInfo, IEnumerable<IHttpRoute>> builder)
#else
		public IConfigureRoutingConventions Build(Func<RouteBuilderInfo, IEnumerable<Route>> builder)
#endif
        {
            builder.MustNotBeNull();
            _lambda.Builder = builder;
            _parent.Add((IBuildRoutes)_lambda);
            return _parent;
        }

#if WEBAPI
        public IConfigureRoutingConventions Modify(Action<IHttpRoute, RouteBuilderInfo> modifier)
#else
		public IConfigureRoutingConventions Modify(Action<Route, RouteBuilderInfo> modifier)
#endif
        {
            modifier.MustNotBeNull();
            _lambda.Modifier = modifier;
            _parent.Add((IModifyRoute)_lambda);
            return _parent;
        }
    }
}