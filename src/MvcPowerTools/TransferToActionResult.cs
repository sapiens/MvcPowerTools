using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcPowerTools
{
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