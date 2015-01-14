using System.Web.Mvc;

namespace MvcPowerTools.ViewEngines.Conventions
{
    /// <summary>
    /// Search for partials in a folder named as the controller and found in the same place as the controller 
    /// </summary>
    public class PartialsInFoldersWithControllerName : PartialsNearController
    {
        public override string GetViewPath(ControllerContext controllerContext, string viewName)
        {
            var nspace = controllerContext.Controller.UrlizeNamespace(true);
            return "~/" + nspace + "/" + viewName + ".cshtml";
        }
    }
}