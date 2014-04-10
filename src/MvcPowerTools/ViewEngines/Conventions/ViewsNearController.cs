using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcPowerTools.ViewEngines.Conventions
{
    /// <summary>
    /// Views are in the same folder as controller. 
    /// View file name pattern is [controller].cshtml if using Handler convention or [controller]_[supplied view].cshtml
    /// </summary>
    public class ViewsNearController : BaseViewConvention
    {
        public override bool Match(ControllerContext context, string viewName)
        {
            return !viewName.StartsWith("_") && !FlexibleViewEngine.IsMvcTemplate(viewName);
        }

        /// <summary>
        /// Gets relative path for view. 
        /// </summary>
        /// <param name="controllerContext"/><param name="viewName"/>
        /// <returns/>
        public override string GetViewPath(ControllerContext controllerContext, string viewName)
        {
            var ctrlType = controllerContext.Controller.GetType();

            var path = ctrlType.ToWebsiteRelativePath(ctrlType.Assembly);
            if (viewName != "Get" && viewName != "Post")
            {
                path =path+"_"+viewName;
            }
            return path + ".cshtml";
        }
    }
}