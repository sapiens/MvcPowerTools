using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Http.Routing.Constraints;

namespace WebApiPowerTools
{
    public class HttpRoutingConventions:IConfigureHttpRoutingConventions
    {
        public static void Configure(Action<HttpRoutingConventions> config,HttpConfiguration httpConfiguration)
        {
            var routing = new HttpRoutingConventions();
            config(routing);
            foreach (var route in routing.Build())
            {
                httpConfiguration.Routes.Add("",route);
            }
            
        }

        public HttpRoutingConventions()
        {
            Actions=new List<HttpActionCall>();
            Conventions=new Dictionary<Predicate<HttpActionCall>, Func<RouteBuilderHelper, IHttpRoute[]>>();
            Constraints=new Dictionary<Type, Func<IHttpRouteConstraint>>();
            AddDefaultConstraints();
        }

        private void AddDefaultConstraints()
        {
            Constrain<int>(()=>new IntRouteConstraint())
                .Constrain<DateTime>(()=> new DateTimeRouteConstraint())
                .Constrain<bool>(()=> new BoolRouteConstraint())
                .Constrain<Guid>(()=>new GuidRouteConstraint());
        }

        public IHttpRoute[] Build()
        {
            var routes = new List<IHttpRoute>();
            foreach (var action in Actions)
            {
                IEnumerable<Func<RouteBuilderHelper, IHttpRoute[]>> conventions = Conventions.Where(d => d.Key(action)).Select(d => d.Value);
                var helper = new RouteBuilderHelper(action, this);
                foreach (var convention in conventions)
                {
                    IHttpRoute[] generated = convention(helper);
                    routes.AddRange(generated);
                }
            }


            return routes.ToArray();
        }

        public IList<HttpActionCall> Actions { get; private set; }

        public IDictionary<Predicate<HttpActionCall>, Func<RouteBuilderHelper, IHttpRoute[]>> Conventions { get;
            private set; }

        public IDictionary<Type, Func<IHttpRouteConstraint>> Constraints { get; private set; }

        public IBuildHttpRoute If(Predicate<HttpActionCall> predicate)
        {
            return new LambdaConventionHost(this,predicate);
        }

        public IConfigureHttpRoutingConventions LoadModule(params HttpRoutingConventionModule[] modules)
        {
            foreach (var module in modules)
            {
                module.Configure(this);
            }
            return this;
        }

        public IConfigureHttpRoutingConventions Constrain<T>(Func<IHttpRouteConstraint> constrainRule)
        {
            if (constrainRule==null) throw new ArgumentNullException("constrainRule");
            Constraints[typeof (T)] = constrainRule;
            return this;
        }

        class LambdaConventionHost : IBuildHttpRoute
        {
            private readonly HttpRoutingConventions _parent;
            private readonly Predicate<HttpActionCall> _predicate;

            public LambdaConventionHost(HttpRoutingConventions parent,Predicate<HttpActionCall> predicate)
            {
                _parent = parent;
                _predicate = predicate;
            }

            public IConfigureHttpRoutingConventions Build(Func<RouteBuilderHelper, IHttpRoute[]> builder)
            {
                _parent.Conventions[_predicate] = builder;
                return _parent;
            }
        }
    }
}