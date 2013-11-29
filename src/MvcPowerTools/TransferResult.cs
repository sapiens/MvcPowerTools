using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcPowerTools
{
    //http://stackoverflow.com/questions/799511/how-to-simulate-server-transfer-in-asp-net-mvc

    /// <summary>
    /// Transfers execution to the supplied url.
    /// </summary>
    public class TransferResult : ActionResult
    {
        public string Url { get; private set; }

        public TransferResult(string url)
        {
            this.Url = url;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var httpContext = HttpContext.Current;

            // MVC 3 running on IIS 7+
            if (HttpRuntime.UsingIntegratedPipeline)
            {
                httpContext.Server.TransferRequest(this.Url, true);
            }
            else
            {
                // Pre MVC 3
                httpContext.RewritePath(this.Url, false);

                IHttpHandler httpHandler = new MvcHttpHandler();
                httpHandler.ProcessRequest(httpContext);
            }
        }
    }

    public class TransferToRouteResult : ActionResult
    {
        public string RouteName { get; set; }
        public RouteValueDictionary RouteValues { get; set; }

        public TransferToRouteResult(RouteValueDictionary routeValues)
            : this(null, routeValues)
        {
        }

        public TransferToRouteResult(string routeName, RouteValueDictionary routeValues)
        {
            this.RouteName = routeName ?? string.Empty;
            this.RouteValues = routeValues ?? new RouteValueDictionary();
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var urlHelper = new UrlHelper(context.RequestContext);
            var url = urlHelper.RouteUrl(this.RouteName, this.RouteValues);

            var actualResult = new TransferResult(url);
            actualResult.ExecuteResult(context);
        }
    }

    public class TransferToActionResult:TransferToRouteResult
    {
        public string ActionName
        {
            get { return RouteValues["action"].As<string>(); }
            set { RouteValues["action"] = value; }
        }
        public string ControllerName
        {
            get { return RouteValues["controller"].As<string>(); }
            set { RouteValues["controller"] = value; }
        }
        
        public TransferToActionResult(RouteValueDictionary routeValues) : base(routeValues)
        {
        }

        public TransferToActionResult(string actionName,string controllerName, RouteValueDictionary routeValues) : base(routeValues)
        {
            ActionName = actionName;
            ControllerName = controllerName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var rt = RouteTable.Routes.GetRouteData(context.HttpContext);
            if (ActionName.IsNullOrEmpty())
            {
                ActionName = rt.GetRequiredString("action");
            }
            
            if (ControllerName.IsNullOrEmpty())
            {
                
                ControllerName = rt.GetRequiredString("controller");
            }
            base.ExecuteResult(context);
        }
    }
}