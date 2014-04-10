using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcPowerTools.ViewEngines.Conventions
{
    /// <summary>
    /// Views are in the same folder as controller. 
    /// View file name pattern is [controller].cshtml if using Handler convention or [controller]_[supplied view].cshtml
    /// </summary>
    public class ViewsBesideController : BaseViewConvention
    {
        private readonly Assembly _appAssembly;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appAssembly">Web application assembly</param>
        public ViewsBesideController(Assembly appAssembly)
        {
            _appAssembly = appAssembly;
        }

        public override bool Match(ControllerContext context, string viewName)
        {
            return !viewName.StartsWith("_");
        }

        /// <summary>
        /// Gets relative path for view. 
        /// </summary>
        /// <param name="controllerContext"/><param name="viewName"/>
        /// <returns/>
        public override string GetViewPath(ControllerContext controllerContext, string viewName)
        {
            var ctrl = controllerContext.Controller.GetType();

            var path = ctrl.ToWebsiteRelativePath(_appAssembly);
            if (viewName != "Get" && viewName != "Post")
            {
                path =path+"_"+viewName;
            }
            return path + ".cshtml";
        }
    }
}