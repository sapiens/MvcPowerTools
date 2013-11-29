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

    /// <summary>
    /// 
    /// </summary>
    public class MvcTemplatesConvention:IFindViewConvention
    {
        public bool Match(ControllerContext context, string viewName)
        {
            return FlexibleViewEngine.IsMvcTemplate(viewName);
        }

        /// <summary>
        /// Gets relative path for view. 
        /// </summary>
        /// <param name="controllerContext"/><param name="viewName"/>
        /// <returns/>
        public string GetViewPath(ControllerContext controllerContext, string viewName)
        {
            var theme = controllerContext.HttpContext.GetThemeInfo();
            if (theme == null)
            {
                return "~/Views/{0}.cshtml".ToFormat(viewName);
            }
            return "{0}/{1}.cshtml".ToFormat(theme.ViewsPath, viewName);
        }

        /// <summary>
        /// Gets relative path for master (layout). If master name is empty, it should return empty
        /// </summary>
        /// <param name="controllerContext"/><param name="masterName"/>
        /// <returns/>
        public string GetMasterPath(ControllerContext controllerContext, string masterName)
        {
            throw new NotSupportedException();
        }
    }
}