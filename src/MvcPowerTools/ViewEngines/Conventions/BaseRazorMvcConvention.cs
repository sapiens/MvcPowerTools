using System;
using System.Web.Mvc;
using MvcPowerTools.Extensions;

namespace MvcPowerTools.ViewEngines.Conventions
{
    /// <summary>
    /// Default asp.net mvc views path searching with theming support
    /// </summary>
    public abstract class BaseRazorMvcConvention:BaseViewConvention
    {
       
        /// <summary>
        /// Gets relative path for view. 
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public override string GetViewPath(ControllerContext controllerContext, string viewName)
        {
            var controller = IsShared?"":controllerContext.RouteData.GetRequiredString("controller");
            var theme = controllerContext.HttpContext.GetThemeInfo();
            var path = "~/Views";
            if (theme!=null)
            {
                path = theme.ViewsPath;                
            }

            return path+"/{0}/{1}.cshtml".ToFormat(IsShared?"Shared":controller,viewName);
            
        }

        /// <summary>
        /// Search in the Shared folder
        /// </summary>
        protected abstract bool IsShared { get; }
               
    }

    
}