using System;
using System.Web.Mvc;
using MvcPowerTools.Extensions;

namespace MvcPowerTools.ViewEngines.Conventions
{
    public abstract class BaseViewConvention:IFindViewConvention
    {
        public virtual bool Match(ControllerContext context, string viewName)
        {
            return true;
        }

        /// <summary>
        /// Gets relative path for view. 
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public abstract string GetViewPath(ControllerContext controllerContext, string viewName);

        /// <summary>
        /// Gets relative path for master (layout). If master name is empty, it should return empty
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="masterName"></param>
        /// <returns></returns>
        public virtual string GetMasterPath(ControllerContext controllerContext, string masterName)
        {
            if (masterName.IsNullOrEmpty()) return masterName;
            var theme = controllerContext.HttpContext.GetThemeInfo();
            var path = "~/Views/";
            if (theme != null)
            {
                path = theme.ViewsPath;
            }

            return path + "{0}/Shared/{1}.cshtml".ToFormat(path,masterName);        
        }
    }
}