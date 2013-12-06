using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;


namespace MvcPowerTools.Routing
{
    public class RoutingConventions:IConfigureRoutingConventions
    {
        public static void Configure(Action<RoutingConventions> cfg)
        {
            cfg.MustNotBeNull();
            var routing = new RoutingConventions();
            cfg(routing);
            routing.Apply(RouteTable.Routes);
        }

        List<IBuildRoutes> _builders=new List<IBuildRoutes>();
        List<IModifyRoute> _modifiers=new List<IModifyRoute>();

        public RoutingConventions()
        {
            Settings = new RoutingConventionsSettings();
            Settings.NamespaceRoot = Assembly.GetCallingAssembly().GetName().Name;
        }

        public void LoadModule(params RoutingConventionsModule[] modules)
        {
            foreach(var m in modules) m.Configure(this);
        }

        public RoutingConventionsSettings Settings { get; private set; }

        private Route _home;
        
        public IConfigureAction If(Predicate<ActionCall> predicate)
        {
           predicate.MustNotBeNull();
            return new LambdaConventionConfigurator(this,predicate);
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

        public IConfigureRoutingConventions HomeIs<T>(Expression<Action<T>> actionSelector) where T : Controller
        {
            actionSelector.MustNotBeNull();
            var routeValues = ControllerExtensions.ToRouteValues(actionSelector);
            _home = new Route(@"{*catch}",routeValues,new MvcRouteHandler());
            return this;
        }

        private Func<ActionCall,IEnumerable<Route>> _defaultBuilder = a => new Route[0];

        public IConfigureRoutingConventions DefaultBuilder(Func<ActionCall,IEnumerable<Route>> builder)
        {
            builder.MustNotBeNull();
            _defaultBuilder = builder;
            return this;
        }

        List<ActionCall> _actions = new List<ActionCall>();
        public void AddAction(ActionCall action)
        {
            action.MustNotBeNull();
            _actions.Add(action);           
        }

        public void Apply(RouteCollection routeCollection)
        {
            foreach (var action in _actions)
            {
                var builder = _builders.FirstOrDefault(d => d.Match(action));
                IEnumerable<Route> routes;
                if (builder != null)
                {
                    routes = builder.Build(action);
                }
                else
                {
                    routes = _defaultBuilder(action);
                }

                foreach (var modifier in _modifiers.Where(d => d.Match(action)))
                {
                    foreach (var route in routes)
                    {
                        modifier.Modify(route,action);
                    }
                }
                foreach (var route in routes)
                {
                    routeCollection.Add(route);
                }
            }
            if (_home != null)
            {
                routeCollection.Add(_home);
            }
        }
    }



    //public class RoutingPolicy
    //{
    //    public RoutingPolicy()
    //    {
    //        //UrlFormatPolicies=new List<IRouteUrlFormatPolicy>();    
    //        //Conventions=new List<IRouteConvention>();
    //        Settings = new RoutingPolicySettings();
    //        Settings.NamespaceRoot = Assembly.GetCallingAssembly().GetName().Name + ".Controllers";
    //        //GlobalPolicies= new List<IRouteGlobalPolicy>();
    //        //EnableDebugInfo = true;
    //    }

    //     /// <summary>
    //     /// Scans assemblies for controllers and policies and apply them to the route collection
    //     /// </summary>
    //     /// <param name="asms"></param>
    //     public static void CreateAndApply(params Assembly[] asms)
    //     {
    //         CreateAndApply(null,asms);
    //     }

    //    /// <summary>
    //    /// Scans assemblies for controllers and policies and apply them to the route collection
    //    /// </summary>
    //    /// <param name="policyConfig">Advanced configuration</param>
    //    /// <param name="asms"></param>
    //    public static void CreateAndApply(Action<RoutingPolicy> policyConfig,params Assembly[] asms)
    //    {
    //        var policy = new RoutingPolicy();
    //        var p = policy.ConfigureFrom(asms);
    //        if (policyConfig != null)
    //        {
    //            policyConfig(p);
    //        }
    //        p.Apply(RouteTable.Routes);
    //    }

    //    public RoutingPolicySettings Settings { get; private set; }
    //    List<ActionCall> _actions=new List<ActionCall>();
    //    public void AddAction(ActionCall action)
    //    {
    //        action.MustNotBeNull();
    //        _actions.Add(action);
            
    //        if (action.Method.HasCustomAttribute<HomepageAttribute>())
    //        {
    //            _homepage = action;
    //        }
    //    }

    //    private ActionCall _homepage;
    //    private bool _enableDebugInfo;

    //    /// <summary>
    //    /// Gets a list of url policies (url format) 
    //    /// which will be applied for any matching action
    //    /// </summary>
    //    public IList<IRouteUrlFormatPolicy> UrlFormatPolicies { get; private set; }
    //    /// <summary>
    //    /// Gets a list of conventions used to create routes
    //    /// </summary>
    //    public IList<IRouteConvention> Conventions { get; private set; }

    //    /// <summary>
    //    /// Gets a list of policies that apply to every route
    //    /// </summary>
    //    public IList<IRouteGlobalPolicy> GlobalPolicies { get; private set; }

    //    public bool EnableDebugInfo
    //    {
    //        get { return _enableDebugInfo; }
    //        set
    //        {
    //            if (value) RoutingPolicy.Debug=new DebugInfo();
    //            _enableDebugInfo = value;
    //        }
    //    }

    //    internal static DebugInfo Debug;

    //    public void Apply(RouteCollection routeCollection)
    //    {
    //       foreach (var action in _actions)
    //        {
    //            foreach (var convention in Conventions.Where(c => c.Match(action)))
    //            {
    //                var routes = convention.Build(action);
    //                foreach (var formatter in UrlFormatPolicies.Where(u => u.Match(action)))
    //                {
    //                    foreach (var route in routes)
    //                    {
    //                        route.Url = formatter.Format(route.Url, action); ;
    //                    }
    //                }

    //                GlobalPolicies.ForEach(p=>routes.ForEach(r=>p.ApplyTo(r)));
    //                if (EnableDebugInfo)
    //                {
    //                    foreach (var rt in routes)
    //                    {
    //                        Debug.Add(action,rt);
    //                    }
    //                }
    //                routes.ForEach(r=>routeCollection.Add(r));                    
    //            }
    //        }
    //        HandleHomepage(routeCollection);
    //    }

    //    void HandleHomepage(RouteCollection routes)
    //    {
    //        if (_homepage == null) return;
    //        var defaults = _homepage.CreateDefaults();
    //        _homepage.SetParamsDefaults(defaults);
    //        var home = _homepage.Method.GetSingleAttribute<HomepageAttribute>();
    //        var route = new Route(home.Url, defaults, new RouteValueDictionary(), _homepage.Settings.CreateHandler());
    //        GlobalPolicies.ForEach(p => p.ApplyTo(route));
    //        routes.Add(route);
    //    }
    //}

    //internal class DebugInfo:Dictionary<ActionCall,Route>
    //{

    //}

    //public class RoutingInfoHandler : IHttpHandler, IRouteHandler
    //{
    //    /// <summary>
    //    /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
    //    /// </summary>
    //    /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
    //    public void ProcessRequest(HttpContext context)
    //    {
    //        var doc = context.Response;
    //        doc.Write("<html><head>");
    //        doc.Write("<title>Routing Convention Info</title>");
    //        doc.Write("</head>");
    //        doc.Write("<body>");
    //        if (RoutingPolicy.Debug != null)
    //        {
    //            doc.Write("<table>");
    //            doc.Write("<thead><tr>");
    //            foreach (var ac in RoutingPolicy.Debug.Keys)
    //            {
    //                doc.Write("<th>Controller</th>");
    //                doc.Write("<th>Url</th>");
    //                doc.Write("<th>Default Values</th>");
    //            }
    //            doc.Write("</tr></thead>");
    //            doc.Write("<tbody>");
    //            foreach (var kv in RoutingPolicy.Debug)
    //            {
    //                doc.Write("<tr>");
    //                doc.Write("<td>");
    //                doc.Write(kv.Key.Controller.Name);
    //                doc.Write("</td>");
                    
    //                doc.Write("<td>");
    //                doc.Write(kv.Value.Url);
    //                doc.Write("</td>");

    //                doc.Write("<td>");
    //                foreach (var dkv in kv.Value.Defaults)
    //                {
    //                    doc.Write(dkv.Key);
    //                    doc.Write(":");
    //                    doc.Write(dkv.Value);
    //                    doc.Write("<br />");
    //                }                    
    //                doc.Write("</td>");

    //                doc.Write("</tr>");
    //            }
    //            doc.Write("</tbody>");
    //            doc.Write("</table>");
    //        }
    //        else
    //        {
    //            doc.Write("<h1>Debugging is not enabled</h1>");
    //        }
    //        doc.Write("</body>");
    //        doc.Write("</html>");
                        
    //    }

    //    /// <summary>
    //    /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
    //    /// </summary>
    //    /// <returns>
    //    /// true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
    //    /// </returns>
    //    public bool IsReusable
    //    {
    //        get
    //        {
    //            return true;
    //        }
    //    }

    //    /// <summary>
    //    /// Provides the object that processes the request.
    //    /// </summary>
    //    /// <returns>
    //    /// An object that processes the request.
    //    /// </returns>
    //    /// <param name="requestContext">An object that encapsulates information about the request.</param>
    //    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    //    {
    //        return this;
    //    }
    //}
}