using System.Web;
using System.Web.Mvc;

namespace MvcPowerTools.ViewEngines.Conventions
{
    public class PartialsNearController : BaseViewConvention
    {
        public override bool Match(ControllerContext context, string viewName)
        {
            return viewName.StartsWith("_") || FlexibleViewEngine.IsMvcTemplate(viewName);
        }

        /// <summary>
        /// Gets relative path for view. 
        /// </summary>
        /// <param name="controllerContext"/><param name="viewName"/>
        /// <returns/>
        public override string GetViewPath(ControllerContext controllerContext, string viewName)
        {
            var nspace = GetNamespace(controllerContext);
            return "~/" + nspace + "/" + viewName + ".cshtml";
        }

        protected string GetNamespace(ControllerContext controllerContext)
        {
            var ctrlType = controllerContext.Controller.GetType();
            var nspace = ctrlType.Namespace.Urlize(ctrlType.Assembly);
            return nspace;
        }
    }
}