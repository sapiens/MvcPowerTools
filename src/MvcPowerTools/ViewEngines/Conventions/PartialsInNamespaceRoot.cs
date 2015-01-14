using System.Web.Mvc;

namespace MvcPowerTools.ViewEngines.Conventions
{
    /// <summary>
    /// Search for partials in the namespace root of the controller.
    /// When controller is in ~/Admin/Users/MyController , the partial should be in ~/Admin
    /// </summary>
    public class PartialsInNamespaceRoot : PartialsNearController
    {
        public override string GetViewPath(ControllerContext controllerContext, string viewName)
        {
            var nspace = GetNamespace(controllerContext).Split('/');
            return "~/" + nspace[0] + "/" + viewName + ".cshtml";
        }

        
    }
}