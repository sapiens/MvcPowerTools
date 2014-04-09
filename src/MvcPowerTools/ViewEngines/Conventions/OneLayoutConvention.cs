using System;
using System.Web.Mvc;
using MvcPowerTools.Extensions;

namespace MvcPowerTools.ViewEngines.Conventions
{
    public abstract class OneLayoutConvention:IFindViewConvention
    {
        public abstract bool Match(ControllerContext context, string viewName);

        /// <summary>
        /// Gets relative path for view. 
        /// </summary>
        /// <param name="controllerContext"/><param name="viewName"/>
        /// <returns/>
        public abstract string GetViewPath(ControllerContext controllerContext, string viewName);

        /// <summary>
        /// Gets relative path for master (layout). If master name is empty, it should return empty
        /// </summary>
        /// <param name="controllerContext"/><param name="masterName"/>
        /// <returns/>
        public virtual string GetMasterPath(ControllerContext controllerContext, string masterName)
        {
            return "";
            if (masterName.IsNullOrEmpty()) return masterName;
            var theme = controllerContext.HttpContext.GetThemeInfo();
            return theme.ViewsPath + "/" + masterName + ".cshtml";
        }
    }
}