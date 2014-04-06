using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Routing.Constraints;
using System.Web.Routing;


namespace MvcPowerTools.Routing
{
    public class RoutingConventions:IConfigureRoutingConventions
    {
        /// <summary>
        /// Configures routing conventions and applies them to RouteTable
        /// </summary>
        /// <param name="cfg"></param>
        public static void Configure(Action<RoutingConventions> cfg)
        {
            cfg.MustNotBeNull();
            var routing = new RoutingConventions();
            cfg(routing);
            var routes=routing.BuildRoutes();
            foreach (var route in routes)
            {
                RouteTable.Routes.Add(route);
            }
        }
        public RoutingConventions()
        {
            Settings = new RoutingConventionsSettings();
            Constraints = new Dictionary<Type, Func<IRouteConstraint>>();
            AddDefaultConstraints();
        }
        private void AddDefaultConstraints()
        {
            Constrain<int>(() => new IntRouteConstraint())
                .Constrain<DateTime>(() => new DateTimeRouteConstraint())
                .Constrain<bool>(() => new BoolRouteConstraint())
                .Constrain<Guid>(() => new GuidRouteConstraint());
        }

        /// <summary>
        /// You can change this. Default is {*catch}
        /// </summary>
        public static string DefaultRouteUrl = @"{*catch}";

        List<IBuildRoutes> _builders=new List<IBuildRoutes>();
        List<IModifyRoute> _modifiers=new List<IModifyRoute>();

      

        public void LoadModule(params RoutingConventionsModule[] modules)
        {
            foreach(var m in modules) m.Configure(this);
        }

        public RoutingConventionsSettings Settings { get; private set; }
      

        public IConfigureAction If(Predicate<ActionCall> predicate)
        {
           predicate.MustNotBeNull();
            return new LambdaConventionConfigurator(this,predicate);
        }
        public IDictionary<Type, Func<IRouteConstraint>> Constraints { get; private set; }

        public IConfigureRoutingConventions Constrain<T>(Func<IRouteConstraint> factory)
        {
            Constraints[typeof (T)] = factory;
            return this;
        }

        public IConfigureRoutingConventions Add(IBuildRoutes convention)
        {
            convention.MustNotBeNull();
            _builders.Add(convention);
            return this;
        }

        public IConfigureRoutingConventions Add(IModifyRoute convention)
        {
            convention.MustNotBeNull();
            _modifiers.Add(convention);
            return this;
        }

        private Route _home;
        public IConfigureRoutingConventions HomeIs<T>(Expression<Action<T>> actionSelector) where T : Controller
        {
            actionSelector.MustNotBeNull();
            var hac=new HomeActionCall(this);
            _home = hac.GetRoute(actionSelector);
       
            return this;
        }

        class HomeActionCall
        {
            private readonly RoutingConventions _parent;
            private ActionCall _action;
            private MethodCallExpression _methodCall;
            private bool _isModelInput;

            private readonly string[] _skipDefaults = new[] {"controller","action","httpMethod"};

            public HomeActionCall(RoutingConventions parent)
            {
                _parent = parent;
            }

            public Route GetRoute<T>(Expression<Action<T>> actionSelector)   where T:Controller
            {
                _methodCall = actionSelector.Body as MethodCallExpression;
                _action = new ActionCall(_methodCall.Method);
                var args=_methodCall.Method.GetParameters();
                if (args.Length == 1)
                {
                    _isModelInput = args[0].ParameterType.IsUserDefinedClass();
                }
                return MakeDefaultRoute();
            }

          
            void FillModelParamValues(IDictionary<string,object> defaults)
            {
                var exprArg = _methodCall.Arguments[0];
                var val = exprArg.GetValue();
                foreach (var prop in val.GetType().GetProperties())
                {
                    if (prop.PropertyType.IsUserDefinedClass()) continue;
                    defaults[prop.Name] = prop.GetValue(val);
                }
            }

            void FillParamValues(IDictionary<string, object> defaults)
            {
                var i = 0;
                foreach (var arg in _methodCall.Method.GetParameters())
                {
                    if (arg.ParameterType.IsUserDefinedClass()) continue;
                    defaults[arg.Name] = _methodCall.Arguments[i].GetValue();
                    i++;
                }                
            }

            Route MakeDefaultRoute()
            {
                var info = new RouteBuilderInfo(_action, _parent);
                var route = info.CreateRoute(DefaultRouteUrl);
                if (_isModelInput)
                {
                    FillModelParamValues(route.Defaults);
                }
                else
                {
                    FillParamValues(route.Defaults);
                }
                route.ConstrainToGet();
                return route;
            } 
        }

        private Func<RouteBuilderInfo, IEnumerable<Route>> _defaultBuilder = a => new Route[0];

        public IConfigureRoutingConventions DefaultBuilder(Func<RouteBuilderInfo, IEnumerable<Route>> builder)
        {
            builder.MustNotBeNull();
            _defaultBuilder = builder;
            return this;
        }

        private List<ActionCall> _actions = new List<ActionCall>();
        public List<ActionCall> Actions
        {
            get { return _actions; }
        }

        
        public IEnumerable<Route> BuildRoutes()
        {
            List<Route>routeCollection=new List<Route>();
            foreach (var action in Actions)
            {
                var helper = new RouteBuilderInfo(action, this);
                var builder = _builders.FirstOrDefault(d => d.Match(action));
                IEnumerable<Route> routes;
                if (builder != null)
                {
                    routes = builder.Build(helper);
                }
                else
                {
                    routes = _defaultBuilder(helper);
                }

                foreach (var modifier in _modifiers.Where(d => d.Match(action)))
                {
                    foreach (var route in routes)
                    {
                        modifier.Modify(route,helper);
                    }
                }

             
                routeCollection.AddRange(routes);
            }
            if (_home != null)
            {
                routeCollection.Add(_home);
            }
            return routeCollection;
        }
    }



  
}