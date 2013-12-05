using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Routing;

namespace MvcPowerTools.Routing
{
    /// <summary>
    /// How to generate routes from actions
    /// </summary>
    public interface IRouteConvention
    {
        bool Match(ActionCall actionCall);
        IEnumerable<Route> Build(ActionCall actionInfo);
    }

    public interface IBuildRoutes:IMatchAction
    {
        IEnumerable<Route> Build(ActionCall actionInfo);
    }

    public interface IModifyRoute : IMatchAction
    {
        void Modify(Route route, ActionCall actionCall);
    }

    public interface IConfigureRoutingConventions
    {
        IConfigureAction If(Predicate<ActionCall> predicate);
        IConfigureRoutingConventions Add(IBuildRoutes convention);
        IConfigureRoutingConventions Add(IModifyRoute convention);
        IConfigureRoutingConventions HomeIs<T>(Expression<Func<T, object>> actionSelector);
    }

    public interface IConfigureAction
    {
        IConfigureRoutingConventions Build(Func<ActionCall, IEnumerable<Route>> builder);
        IConfigureRoutingConventions Modify(Action<Route,ActionCall> modifier);
    }

    class LambdaConventionHost : IBuildRoutes,IModifyRoute
    {
        private readonly Predicate<ActionCall> _predicate;

        public LambdaConventionHost(Predicate<ActionCall> predicate)
        {
            _predicate = predicate;
        }

        public bool Match(ActionCall action)
        {
            return _predicate(action);
        }

        public Func<ActionCall, IEnumerable<Route>> Builder { get; set; }
        public Action<Route,ActionCall> Modifier { get; set; }

        public void Modify(Route route, ActionCall actionCall)
        {
            if (Modifier != null)
            {
                Modifier(route, actionCall);
            }
        }

        public IEnumerable<Route> Build(ActionCall actionInfo)
        {
            if (Builder != null)
            {
                return Builder(actionInfo);
            }
            return new Route[0];
        }
    }

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