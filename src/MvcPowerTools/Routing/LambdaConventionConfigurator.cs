using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace MvcPowerTools.Routing
{
    class LambdaConventionConfigurator:IConfigureAction
    {
        private readonly IConfigureRoutingConventions _parent;
        private LambdaConventionHost _lambda;

        public LambdaConventionConfigurator(IConfigureRoutingConventions parent,Predicate<ActionCall> predicate)
        {
            _parent = parent;
            _lambda = new LambdaConventionHost(predicate);
        }

        public IConfigureRoutingConventions Build(Func<ActionCall, IEnumerable<Route>> builder)
        {
            builder.MustNotBeNull();
            _lambda.Builder = builder;
            _parent.Add((IBuildRoutes)_lambda);
            return _parent;
        }

        public IConfigureRoutingConventions Modify(Action<Route, ActionCall> modifier)
        {
            modifier.MustNotBeNull();
            _lambda.Modifier = modifier;
            _parent.Add((IModifyRoute) _lambda);
            return _parent;
        }

        
    }
}