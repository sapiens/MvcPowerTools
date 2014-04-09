using System;
using System.Web.Mvc;
using MvcPowerTools.Extensions;

namespace MvcPowerTools.ViewEngines.Conventions
{
    /// <summary>
    /// Used for Handler pattern controllers
    /// </summary>
    public class ViewIsControllerNameConvention:OneLayoutConvention
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
            var theme = controllerContext.HttpContext.GetThemeInfo();
            var controller = controllerContext.RouteData.GetRequiredString("controller");
            if (theme==null)
            {
                return "~/Views/{0}.cshtml".ToFormat(controller);
            }

            return "{0}/{1}.cshtml".ToFormat(theme.ViewsPath,controller);
        }
        
    }
}